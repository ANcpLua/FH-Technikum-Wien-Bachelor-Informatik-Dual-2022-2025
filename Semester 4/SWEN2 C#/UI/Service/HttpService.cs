using System.Net;
using System.Net.Http.Json;
using UI.Decorator;
using UI.Service.Interface;
using ILogger=Serilog.ILogger;

namespace UI.Service;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly TryCatchToastWrapper _tryCatchToastWrapper;

    public HttpService(
        HttpClient httpClient,
        IToastServiceWrapper toastServiceWrapper,
        ILogger logger
    )
    {
        _httpClient = httpClient;
        _tryCatchToastWrapper = new TryCatchToastWrapper(toastServiceWrapper, logger);
    }

    [UiMethodDecorator]
    public Task<T?> GetAsync<T>(string uri) => SendRequestAsync<T>(HttpMethod.Get, uri);

    [UiMethodDecorator]
    public Task<IEnumerable<T>?> GetListAsync<T>(string uri) => SendRequestAsync<IEnumerable<T>>(HttpMethod.Get, uri);

    [UiMethodDecorator]
    public Task<T?> PostAsync<T>(string uri, object? data) => SendRequestAsync<T>(HttpMethod.Post, uri, data);

    [UiMethodDecorator]
    public Task<T?> PutAsync<T>(string uri, object? data) => SendRequestAsync<T>(HttpMethod.Put, uri, data);

    [UiMethodDecorator]
    public Task DeleteAsync(string uri) => SendRequestAsync(HttpMethod.Delete, uri);

    [UiMethodDecorator]
    public Task<string?> GetStringAsync(string uri) => SendRequestAsync(
    HttpMethod.Get,
    uri,
    responseHandler: response => response.Content.ReadAsStringAsync()
    );

    [UiMethodDecorator]
    public Task<byte[]?> GetByteArrayAsync(string uri) => SendRequestAsync(
    HttpMethod.Get,
    uri,
    responseHandler: response => response.Content.ReadAsByteArrayAsync(),
    errorHandler: ex => {
        if (ex is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            throw new Exception($"The requested resource was not found: {uri}");
        }
        throw new HttpRequestException($"Error fetching data from {uri}", ex);
    }
    );

    [UiMethodDecorator]
    public Task PostAsync(string uri, object? data) => SendRequestAsync(HttpMethod.Post, uri, data);

    private Task<T?> SendRequestAsync<T>(
        HttpMethod method,
        string uri,
        object? data = null,
        Func<HttpResponseMessage, Task<T>>? responseHandler = null,
        Action<Exception>? errorHandler = null
    ) => _tryCatchToastWrapper.ExecuteAsync(
    async () => {
        var request = new HttpRequestMessage(method, uri);
        if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put))
        {
            request.Content = JsonContent.Create(data);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        if (responseHandler != null)
        {
            return await responseHandler(response);
        }
        return await response.Content.ReadFromJsonAsync<T>();
    },
    $"Error {method} data {(method == HttpMethod.Get ? "from" : "to")} {uri}",
    errorHandler
    );

    private Task SendRequestAsync(HttpMethod method, string uri, object? data = null) => _tryCatchToastWrapper.ExecuteAsync(
    async () => {
        var request = new HttpRequestMessage(method, uri);
        if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put))
        {
            request.Content = JsonContent.Create(data);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    },
    $"Error {method} data {(method == HttpMethod.Get ? "from" : "to")} {uri}"
    );
}
