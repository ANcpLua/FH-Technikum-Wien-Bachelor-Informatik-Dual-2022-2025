using Minio;
using PaperlessServices.MinIoStorage;

namespace PaperlessServices.Extensions;

public static class MinioStorageServiceCollectionExtensions
{
    public static void AddMinioObjectStorage(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var bucketName = configuration["MinIO:BucketName"];
        if (string.IsNullOrEmpty(bucketName))
            throw new InvalidOperationException("MinIO BucketName is not configured. Ensure MinIO:BucketName is set in service-appsettings.json");

        services.AddSingleton<MinioClient>(_ =>
        {
            var env = environment.EnvironmentName;
            var endpoint = env == "Docker" ? "minio:9000" : "localhost:9000";
            return (MinioClient)new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(
                    configuration["MinIO:AccessKey"],
                    configuration["MinIO:SecretKey"])
                .WithSSL(false)
                .Build();
        });

        services.AddScoped<IMinioStorageService, MinioStorageService>();
    }
}