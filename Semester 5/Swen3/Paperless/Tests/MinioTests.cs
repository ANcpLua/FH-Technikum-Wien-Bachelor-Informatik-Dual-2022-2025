using Contract.Logger;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Tests;

[TestFixture]
public class MinioTests
{
    private MinioClient _minioClient;
    private IConfiguration _configuration;
    private IPaperlessLogger _logger;
    private const string TestBucket = "documents";
    private const string TestFileName = "HelloWorld.pdf";

    private readonly string _testFilePath =
        Path.Combine(TestContext.CurrentContext.TestDirectory, "EndToEndTests", "HelloWorld.pdf");

    [SetUp]
    public async Task Setup()
    {
        // Load configuration
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("service-appsettings.json")
            .AddEnvironmentVariables();

        _configuration = configBuilder.Build();

        // Initialize MinIO client
        _minioClient = (MinioClient)new MinioClient()
            .WithEndpoint(_configuration["MinIO:Endpoint"] ?? "localhost:9000")
            .WithCredentials(
                _configuration["MinIO:AccessKey"] ?? "minioadmin",
                _configuration["MinIO:SecretKey"] ?? "minioadmin")
            .Build();

        // Initialize logger
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        _logger = new PaperlessLogger(loggerFactory.CreateLogger("MinioTests"), "MinioTests", "Test");

        // Ensure bucket exists
        bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(TestBucket));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(TestBucket));
            _logger.LogOperation("Minio", "Setup", $"Created bucket: {TestBucket}");
        }
    }

    [Test, Order(1)]
    public async Task UploadFile_ToMinio_Succeeds()
    {
        // Arrange
        await using var fileStream = File.OpenRead(_testFilePath);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(TestBucket)
            .WithObject(TestFileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType("application/pdf");

        // Act
        await _minioClient.PutObjectAsync(putObjectArgs);


        // Verify the object exists
        var stat = await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(TestBucket)
            .WithObject(TestFileName));

        // Assert
        stat.Should().NotBeNull();
        stat.ObjectName.Should().Be(TestFileName);
        _logger.LogOperation("Minio", "UploadFile", $"Uploaded file: {TestFileName}");
    }

    [Test, Order(2)]
    public async Task GetFile_FromMinio_Succeeds()
    {
        // Arrange
        using var memoryStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(TestBucket)
            .WithObject(TestFileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
            });

        // Act
        await _minioClient.GetObjectAsync(getObjectArgs);

        // Assert
        memoryStream.Length.Should().BeGreaterThan(0);
        memoryStream.Position = 0;

        await using var downloadedStream = File.OpenRead(_testFilePath);
        downloadedStream.Length.Should().Be(memoryStream.Length);
        _logger.LogOperation("Minio", "GetFile", $"Downloaded file: {TestFileName}");
    }

    [Test, Order(3)]
    public async Task DeleteFile_FromMinio_Succeeds()
    {
        // Arrange
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(TestBucket)
            .WithObject(TestFileName);

        // Act
        await _minioClient.RemoveObjectAsync(removeObjectArgs);


        // Verify the object no longer exists
        Func<Task> act = async () => await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(TestBucket)
            .WithObject(TestFileName));

        // Assert
        await act.Should().ThrowAsync<Minio.Exceptions.ObjectNotFoundException>();
        _logger.LogOperation("Minio", "DeleteFile", $"Deleted file: {TestFileName}");
    }
}
