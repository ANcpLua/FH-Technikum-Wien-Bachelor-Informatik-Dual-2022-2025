namespace UI.Service.Interface;

public interface IHttpService
{
    Task<T?> GetAsync<T>(string uri);
    Task<IEnumerable<T>?> GetListAsync<T>(string uri);
    Task<T?> PostAsync<T>(string uri, object? data);
    Task<T?> PutAsync<T>(string uri, object? data);
    Task DeleteAsync(string uri);
    Task<string?> GetStringAsync(string uri);
    Task<byte[]?> GetByteArrayAsync(string uri);
    Task PostAsync(string uri, object? data);
}