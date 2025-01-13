using Contract;

namespace PaperlessServices.BL;

public interface IDocumentService
{
    Task<DocumentDto> Upload(DocumentDto uploadDto, CancellationToken cancellationToken);
    Task<DocumentDto> GetDocument(int id, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentDto>> GetAllDocuments(CancellationToken cancellationToken);
    Task<DocumentDto> UpdateDocument(DocumentDto documentDto, CancellationToken cancellationToken);
    Task DeleteDocument(int id, CancellationToken cancellationToken);
}