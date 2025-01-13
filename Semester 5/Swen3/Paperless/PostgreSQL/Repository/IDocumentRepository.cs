using Microsoft.EntityFrameworkCore.Storage;
using PostgreSQL.Entities;

namespace PostgreSQL.Repository;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<Document> Upload(Document document, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Document> UpdateAsync(Document document, CancellationToken cancellationToken = default);
}