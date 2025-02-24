using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UI.Decorator;
using UI.Service.Interface;

namespace UI.Service;

public class RouteApiService : IRouteApiService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public RouteApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [UiMethodDecorator]
    public async Task<(double Distance, double Duration)> FetchRouteDataAsync(
        (double Latitude, double Longitude) from,
        (double Latitude, double Longitude) to,
        string transportType
    )
    {
        var endpoint = GetEndpointForTransportType(transportType);
        var apiKey = _configuration["AppSettings:OpenRouteServiceApiKey"];
        var baseUrl = _configuration["AppSettings:OpenRouteServiceApiBaseUrl"];

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
        );
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Bearer",
        apiKey
        );

        var payload = new
        {
            coordinates = new List<List<double>>
            {
                new()
                {
                    from.Longitude, from.Latitude
                },
                new()
                {
                    to.Longitude, to.Latitude
                }
            }
        };

        StringContent content = new(
        JsonSerializer.Serialize(payload),
        Encoding.UTF8,
        "application/json"
        );

        var response = await _httpClient.PostAsync($"{baseUrl}/v2/directions/{endpoint}", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
            $"Error fetching route data. Status code: {response.StatusCode}, Content: {errorContent}"
            );
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return ParseRouteData(jsonString);
    }

    private static string GetEndpointForTransportType(string transportType) => transportType switch
    {
        "Car" => "driving-car",
        "Bike" => "cycling-regular",
        "Foot" => "foot-walking",
        _ => "driving-car"
    };

    private static (double Distance, double Duration) ParseRouteData(string jsonString)
    {
        using var json = JsonDocument.Parse(jsonString);
        var route = json.RootElement.GetProperty("routes")[0];
        var summary = route.GetProperty("summary");

        var distance = summary.GetProperty("distance").GetDouble();
        var duration = summary.GetProperty("duration").GetDouble();

        return (distance, duration);
    }
}