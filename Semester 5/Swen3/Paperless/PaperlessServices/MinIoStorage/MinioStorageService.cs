using Contract.Logger;
using Minio;
using Minio.DataModel.Args;

namespace PaperlessServices.MinIoStorage;

public class MinioStorageService : IMinioStorageService
{
    private readonly MinioClient _minioClient;
    private readonly string? _bucketName;
    private readonly IPaperlessLogger _logger;

    public MinioStorageService(
        MinioClient minioClient, 
        IConfiguration configuration,
        IPaperlessLogger logger)
    {
        _minioClient = minioClient;
        _bucketName = configuration["MinIO:BucketName"];
        _logger = logger;
        
        EnsureBucketExistsAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<string> UploadFileAsync(string fileName, Stream stream, CancellationToken cancellationToken)
    {
        try
        {
            // Ensure the bucket exists before attempting to upload a file.
            await EnsureBucketExistsAsync(cancellationToken);
            
            // Configure the arguments required for uploading a file to MinIO.
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream");

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            _logger.LogOperation("Storage", "Upload", $"File: {fileName}, Bucket: {_bucketName}");
            
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError("Storage", "Upload", $"Failed to upload {fileName} to {_bucketName}", ex);
            throw;
        }
    }

    public async Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var memoryStream = new MemoryStream();
            
            // Download the file from MinIO to a memory stream for in-memory operations.
            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream)),
                cancellationToken);

            // Reset the stream position to the beginning for consumers.
            memoryStream.Position = 0;

            _logger.LogOperation("Storage", "Download", $"File: {fileName}, Bucket: {_bucketName}");
            
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError("Storage", "Download", $"Failed to download {fileName} from {_bucketName}", ex);
            throw;
        }
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Check if the bucket exists in MinIO.
            var exists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName),
                cancellationToken);

            // Create the bucket if it does not exist.
            if (!exists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName),
                    cancellationToken);
                    
                _logger.LogOperation("Storage", "CreateBucket", $"Created new bucket: {_bucketName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Storage", "EnsureBucket", $"Failed to ensure bucket {_bucketName} exists", ex);
            throw;
        }
    }
}