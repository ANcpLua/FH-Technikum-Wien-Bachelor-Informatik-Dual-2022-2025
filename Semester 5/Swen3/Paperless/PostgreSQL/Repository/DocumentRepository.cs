using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostgreSQL.Data;
using PostgreSQL.Entities;

namespace PostgreSQL.Repository;

public class DocumentRepository : IDocumentRepository
{
    private readonly PaperlessDbContext _context;
    private readonly ILogger<DocumentRepository> _logger;
    private readonly string _environment;

    public DocumentRepository(
        PaperlessDbContext context,
        ILogger<DocumentRepository> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Unknown";
    }

    public async Task<Document?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Document with ID: {DocumentId} in {Environment} environment", 
            id, _environment);

        if (IsDetailedLoggingEnabled())
        {
            await LogTableContents(cancellationToken);
        }

        var document = await _context.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        LogDocumentResult(document, id);
        return document;
    }

    public async Task<IEnumerable<Document>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all documents in {Environment} environment", _environment);

        var documents = await _context.Documents
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {DocumentCount} documents in {Environment} environment",
            documents.Count, _environment);

        return documents;
    }

    public async Task<Document> Upload(Document document, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new Document to the database");
        _context.Documents.Add(document);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Document saved with ID: {DocumentId}", document.Id);
        return document;
    }

    public async Task<Document> UpdateAsync(Document document, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating document with ID: {DocumentId}", document.Id);
        _context.Documents.Update(document);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Document with ID {DocumentId} updated successfully", document.Id);
        return document;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var document = await GetByIdAsync(id, cancellationToken);
        if (document == null)
        {
            _logger.LogWarning("Attempted to delete non-existent document with ID {DocumentId}", id);
            return;
        }

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Document with ID {DocumentId} deleted successfully", id);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    private bool IsDetailedLoggingEnabled() 
        => _environment is "Development" or "Docker";

    private async Task LogTableContents(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Detailed logging enabled. Checking database connection and table contents");
        
        var tableContents = await _context.Documents
            .AsNoTracking()
            .Select(d => new { d.Id, d.Name, d.FilePath })
            .ToListAsync(cancellationToken);
        
        _logger.LogInformation("Current Documents in table: {Documents}",
            string.Join(", ", tableContents.Select(d => $"ID: {d.Id}, Name: {d.Name}")));
    }

    private void LogDocumentResult(Document? document, int id)
    {
        if (document == null)
        {
            _logger.LogWarning("No Document found with ID: {DocumentId} in {Environment} environment",
                id, _environment);
            return;
        }

        _logger.LogInformation("Found Document with ID: {DocumentId}, Name: {DocumentName} in {Environment} environment",
            document.Id, document.Name, _environment);
    }
}