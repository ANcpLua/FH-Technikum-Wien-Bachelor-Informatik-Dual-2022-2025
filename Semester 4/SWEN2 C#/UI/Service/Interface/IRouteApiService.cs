namespace UI.Service.Interface;

public interface IRouteApiService
{
    Task<(double Distance, double Duration)> FetchRouteDataAsync(
        (double Latitude, double Longitude) from,
        (double Latitude, double Longitude) to,
        string transportType
    );
}
