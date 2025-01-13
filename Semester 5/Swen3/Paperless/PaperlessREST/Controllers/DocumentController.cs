using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Contract;
using Contract.Logger;
using EasyNetQ;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using PaperlessServices.BL;
using PaperlessServices.MinIoStorage;

namespace PaperlessREST.Controllers;

[ApiController]
[Route("documents")]
[Produces("application/json")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IMinioStorageService _minioStorageService;
    private readonly IPaperlessLogger _logger;
    private readonly ElasticsearchClient _elasticClient;
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public DocumentController(
        IDocumentService documentService,
        IMinioStorageService minioStorageService,
        IPaperlessLogger logger,
        ElasticsearchClient elasticClient,
        IBus bus,
        IMapper mapper) 
    {
        _documentService = documentService;
        _minioStorageService = minioStorageService;
        _logger = logger;
        _elasticClient = elasticClient;
        _bus = bus;
        _mapper = mapper;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentDto>> Upload([FromForm][Required] string name, [Required] IFormFile file, CancellationToken cancellationToken)
    {
        _logger.LogOperation("Upload", "StartUpload", $"Received upload request for document: {name}");

        var documentDto = new DocumentDto { Name = name, File = file };
        var result = await _documentService.Upload(documentDto, cancellationToken);
        await _bus.PubSub.PublishAsync(new DocumentUploadedEvent
        {
            DocumentId = result.Id,
            FileName = result.FilePath,
            UploadedAt = result.DateUploaded
        }, cancellationToken);

        _logger.LogOperation("Upload", "UploadComplete", $"Document uploaded successfully: {result.Id}");
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DocumentDto>> Get([FromRoute][Required] int id, CancellationToken cancellationToken)
    {
        var document = await _documentService.GetDocument(id, cancellationToken);
        return Ok(document);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var documents = await _documentService.GetAllDocuments(cancellationToken);
        return Ok(documents);
    }

    [HttpGet("{id}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Download([FromRoute][Required] int id, CancellationToken cancellationToken)
    {
        _logger.LogOperation("Download", "StartDownload", $"Download requested for document: {id}");

        var document = await _documentService.GetDocument(id, cancellationToken);
        var fileStream = await _minioStorageService.GetFileAsync(document.FilePath, cancellationToken);

        _logger.LogOperation("Download", "DownloadComplete", $"Download successful for document: {id}");
        return File(fileStream, "application/octet-stream", document.Name);
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> SearchDocuments([FromQuery][Required] string query, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<DocumentDto>(s => s
                .Index("paperless-documents")
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(query)
                        .Fields(new[] { "name", "ocrText" })
                        .Fuzziness(new Fuzziness("AUTO"))
                        .MinimumShouldMatch("75%"))),
            cancellationToken);

        if (response.IsValidResponse)
        {
            var documents = _mapper.Map<IEnumerable<DocumentDto>>(response.Documents);
            return Ok(new
            {
                totalHits = response.Total,
                documents
            });
        }

        _logger.LogOperation("Search", "SearchFailed", $"Search failed for query: {query}");
        throw new InvalidOperationException("Search operation failed");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute][Required] int id, CancellationToken cancellationToken)
    {
        _logger.LogOperation("Delete", "StartDelete", $"Delete requested for document: {id}");

        await _documentService.DeleteDocument(id, cancellationToken);

        _logger.LogOperation("Delete", "DeleteComplete", $"Document deleted successfully: {id}");
        return NoContent();
    }
}
