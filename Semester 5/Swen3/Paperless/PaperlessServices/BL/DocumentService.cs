using AutoMapper;
using Contract;
using Contract.Logger;
using EasyNetQ;
using Elastic.Clients.Elasticsearch;
using FluentValidation;
using FluentValidation.Results;
using Npgsql;
using PostgreSQL.Entities;
using PaperlessServices.Entities;
using PaperlessServices.MinIoStorage;
using PostgreSQL.Repository;

namespace PaperlessServices.BL;

public class DocumentService : IDocumentService
{
    private readonly IMinioStorageService _minioStorageService;
    private readonly IDocumentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPaperlessLogger _logger;
    private readonly ElasticsearchClient _elasticClient;
    private readonly IBus _messageBus;
    private readonly IConfiguration _configuration;
    private readonly IValidator<BlDocument> _validator;

    public DocumentService(
        IMinioStorageService minioStorageService,
        IDocumentRepository repository,
        IMapper mapper,
        IPaperlessLogger logger,
        ElasticsearchClient elasticClient,
        IBus messageBus,
        IConfiguration configuration,
        IValidator<BlDocument> validator)
    {
        _minioStorageService = minioStorageService;
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _elasticClient = elasticClient;
        _messageBus = messageBus;
        _configuration = configuration;
        _validator = validator;
    }

    public async Task<DocumentDto> Upload(DocumentDto documentDto, CancellationToken cancellationToken)
    {
        _logger.LogOperation("Upload", "Start", $"Starting document upload for {documentDto.Name}");

        if (documentDto.File == null)
        {
            _logger.LogError("Upload", "Validation", "File is required for upload",
                new ArgumentException("File is required for upload"));
            throw new ArgumentException("File is required for upload");
        }

        // Begin a database transaction to ensure atomicity
        await using var transaction = await _repository.BeginTransactionAsync(cancellationToken);

        try
        {
            // Create a new document entity
            var document = new Document
            {
                Name = documentDto.File.FileName,
                FilePath = "temp",
                DateUploaded = DateTime.UtcNow
            };

            // Map to BlDocument for validation
            var blDocument = _mapper.Map<BlDocument>(document);

            // Validate the BlDocument entity
            ValidationResult validationResult = await _validator.ValidateAsync(blDocument, cancellationToken);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError("Upload", "Validation", error.ErrorMessage, new ValidationException(error.ErrorMessage));
                }
                throw new ValidationException("Document validation failed.");
            }

            // Save the document to the database
            var savedDocument = await _repository.Upload(document, cancellationToken);
            _logger.LogOperation("Upload", "PostgreSQL", $"Document saved with ID: {savedDocument.Id}");

            // Generate a unique file name and upload the file to storage
            var fileName = GenerateDocumentName(documentDto.File.FileName, savedDocument.Id);
            await using var stream = documentDto.File.OpenReadStream();
            await _minioStorageService.UploadFileAsync(fileName, stream, cancellationToken);
            _logger.LogOperation("Upload", "MinIO", $"File uploaded: {fileName}");

            // Update the document with the file path and re-save in the database
            savedDocument.FilePath = fileName;
            savedDocument.DateUploaded = DateTime.UtcNow;

            // Update BlDocument for validation before updating
            blDocument = _mapper.Map<BlDocument>(savedDocument);

            // Validate again after updating file path
            validationResult = await _validator.ValidateAsync(blDocument, cancellationToken);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError("Upload", "Validation", error.ErrorMessage, new ValidationException(error.ErrorMessage));
                }
                throw new ValidationException("Document validation failed after file upload.");
            }

            await _repository.UpdateAsync(savedDocument, cancellationToken);
            _logger.LogOperation("Upload", "PostgreSQL", $"Document metadata updated for ID: {savedDocument.Id}");

            // Map the document entity to DTO and index it in Elasticsearch
            var mappedDocument = _mapper.Map<BlDocument>(savedDocument);
            var dto = _mapper.Map<DocumentDto>(mappedDocument);

            await IndexDocument(dto, cancellationToken);
            _logger.LogOperation("Upload", "Elastic", $"Document indexed with ID: {savedDocument.Id}");

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            // Publish an event to notify about the upload
            var documentUploadedEvent = new DocumentUploadedEvent
            {
                DocumentId = dto.Id,
                FileName = dto.FilePath,
                UploadedAt = dto.DateUploaded
            };

            await _messageBus.PubSub.PublishAsync(documentUploadedEvent, "document.uploaded", cancellationToken);
            _logger.LogOperation("Upload", "Event", $"Published event for DocumentId {dto.Id}");

            return dto;
        }
        catch (ValidationException vex)
        {
            // Rollback transaction in case of validation errors
            _logger.LogError("Upload", "ValidationFailed", "Validation failed during document upload. Rolling back changes...", vex);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            // Rollback transaction in case of other errors
            _logger.LogError("Upload", "Failed", "Error occurred while uploading document. Rolling back changes...", ex);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private string GenerateDocumentName(string originalName, int documentId)
    {
        return $"{documentId}_{originalName}";
    }

    private async Task IndexDocument(DocumentDto document, CancellationToken cancellationToken)
    {
        try
        {
            var indexResponse = await _elasticClient.IndexAsync(document,
                i => i.Index("paperless-documents")
                    .Id(document.Id.ToString())
                    .Refresh(Refresh.True),
                cancellationToken);

            if (!indexResponse.IsValidResponse)
            {
                var error = $"Failed to index document: {indexResponse.DebugInformation}";
                _logger.LogError("Index", "Failed", error,
                    new Exception(error));
                throw new Exception(error);
            }

            _logger.LogOperation("Index", "Success", $"Document {document.Id} indexed in Elasticsearch");
        }
        catch (Exception ex)
        {
            _logger.LogError("Index", "Error", $"Error indexing document {document.Id}", ex);
            throw;
        }
    }

    public async Task<DocumentDto> UpdateDocument(DocumentDto documentDto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogOperation("Update", "Start", $"Updating document {documentDto.Id}");

            // Retrieve the existing document by ID from the repository
            var document = await _repository.GetByIdAsync(documentDto.Id, cancellationToken);
            if (document == null)
            {
                _logger.LogError("Update", "NotFound", $"Document with ID {documentDto.Id} not found",
                    new KeyNotFoundException($"Document with ID {documentDto.Id} not found"));
                throw new KeyNotFoundException($"Document with ID {documentDto.Id} not found");
            }

            // Update the document properties with the new values from the DTO
            document.Name = documentDto.Name;
            document.OcrText = documentDto.OcrText;

            // Map to BlDocument for validation
            var blDocument = _mapper.Map<BlDocument>(document);

            // Validate the updated BlDocument entity
            ValidationResult validationResult = await _validator.ValidateAsync(blDocument, cancellationToken);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError("Update", "Validation", error.ErrorMessage, new ValidationException(error.ErrorMessage));
                }
                throw new ValidationException("Document validation failed during update.");
            }

            // Save the updated document back to the database
            var updatedDocument = await _repository.UpdateAsync(document, cancellationToken);
            _logger.LogOperation("Update", "PostgreSQL", $"Document {documentDto.Id} updated in database");

            // Map the updated database entity to the business logic and DTO layers
            var mappedDocument = _mapper.Map<BlDocument>(updatedDocument);
            var dto = _mapper.Map<DocumentDto>(mappedDocument);

            // Re-index the updated document in Elasticsearch
            await IndexDocument(dto, cancellationToken);
            _logger.LogOperation("Update", "Complete", $"Document {documentDto.Id} successfully updated");

            // Return the updated document as a DTO
            return dto;
        }
        catch (ValidationException vex)
        {
            _logger.LogError("Update", "ValidationFailed", $"Validation failed while updating document {documentDto.Id}", vex);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Update", "Failed", $"Failed to update document {documentDto.Id}", ex);
            throw;
        }
    }

    public async Task<DocumentDto> GetDocument(int id, CancellationToken cancellationToken)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            _logger.LogOperation("Get", "Database",
                $"Using host: {new NpgsqlConnectionStringBuilder(connectionString).Host}, " +
                $"database: {new NpgsqlConnectionStringBuilder(connectionString).Database}");

            _logger.LogOperation("Get", "Start", $"Retrieving Document with ID: {id}");
            var document = await _repository.GetByIdAsync(id, cancellationToken);

            if (document == null)
            {
                _logger.LogError("Get", "NotFound", $"Document with ID {id} not found",
                    new KeyNotFoundException($"Document with ID {id} not found"));
                throw new KeyNotFoundException($"Document with ID {id} not found");
            }

            _logger.LogOperation("Get", "Success", $"Document with ID {id} retrieved");
            var mappedDocument = _mapper.Map<BlDocument>(document);
            var dto = _mapper.Map<DocumentDto>(mappedDocument);

            return dto;
        }
        catch (Exception ex) when (!(ex is KeyNotFoundException))
        {
            _logger.LogError("Get", "Failed", $"Error retrieving document {id}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<DocumentDto>> GetAllDocuments(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogOperation("GetAll", "Start", "Retrieving all documents");

            var documents = await _repository.GetAllAsync(cancellationToken);
            var mappedDocs = _mapper.Map<IEnumerable<BlDocument>>(documents);

            // Validate all documents before mapping to DTOs
            foreach (var blDoc in mappedDocs)
            {
                ValidationResult validationResult = await _validator.ValidateAsync(blDoc, cancellationToken);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogError("GetAll", "Validation", $"Document ID {blDoc.Id}: {error.ErrorMessage}", new ValidationException(error.ErrorMessage));
                    }
                    throw new ValidationException("One or more documents failed validation.");
                }
            }

            var dtos = _mapper.Map<List<DocumentDto>>(mappedDocs).AsReadOnly();

            // (O(1) since it's a List)
            _logger.LogOperation("GetAll", "Success", $"Retrieved {dtos.Count} documents");

            return dtos;
        }
        catch (ValidationException vex)
        {
            _logger.LogError("GetAll", "ValidationFailed", "Validation failed while retrieving all documents", vex);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("GetAll", "Failed", "Error retrieving all documents", ex);
            throw;
        }
    }

    public async Task DeleteDocument(int id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogOperation("Delete", "Start", $"Deleting document {id}");

            var documentEntity = await _repository.GetByIdAsync(id, cancellationToken);
            if (documentEntity == null)
            {
                _logger.LogError("Delete", "NotFound", $"Document with ID {id} not found",
                    new KeyNotFoundException($"Document with ID {id} not found"));
                throw new KeyNotFoundException($"Document with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogOperation("Delete", "PostgreSQL", $"Document {id} deleted from database");

            var deleteResponse = await _elasticClient.DeleteAsync<DocumentDto>(
                id.ToString(),
                idx => idx.Index("paperless-documents"),
                cancellationToken);

            if (!deleteResponse.IsValidResponse)
            {
                var error = $"Failed to delete document {id} from Elasticsearch: {deleteResponse.DebugInformation}";
                _logger.LogError("Delete", "ElasticSearch", error, new Exception(error));
                throw new Exception(error);
            }

            _logger.LogOperation("Delete", "Success", $"Document {id} completely deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError("Delete", "Failed", $"Failed to delete document {id}", ex);
            throw;
        }
    }
}
