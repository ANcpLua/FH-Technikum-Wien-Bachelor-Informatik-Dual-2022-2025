using Elastic.Clients.Elasticsearch;

namespace PaperlessServices.Extensions;

public static class ElasticSearchServiceCollectionExtensions
{
    public static void AddElasticSearchEngine(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ElasticsearchClient>(_ =>
        {
            var elasticUri = configuration["Elasticsearch:Url"] ?? "http://localhost:9200";
            var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
                .DefaultIndex("paperless-documents");
            return new ElasticsearchClient(settings);
        });
    }
}