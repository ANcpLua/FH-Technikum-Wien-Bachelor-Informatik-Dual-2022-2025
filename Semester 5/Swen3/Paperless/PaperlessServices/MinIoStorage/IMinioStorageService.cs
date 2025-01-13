namespace PaperlessServices.MinIoStorage;

public interface IMinioStorageService
{
    Task<string> UploadFileAsync(string fileName, Stream stream, CancellationToken cancellationToken);
    Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken);
}