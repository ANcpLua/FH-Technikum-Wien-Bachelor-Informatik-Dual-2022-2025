using PaperlessServices.BL;

namespace PaperlessServices.Extensions;

public static class DocumentProcessingServiceCollectionExtensions
{
    public static void AddDocumentProcessing(this IServiceCollection services)
    {
        services.AddScoped<IDocumentService, DocumentService>();
    }
}