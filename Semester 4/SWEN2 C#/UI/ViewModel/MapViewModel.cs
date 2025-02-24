using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.ViewModel;

public class MapViewModel : BaseViewModel
{
    public static readonly Dictionary<string, (double Latitude, double Longitude)> Coordinates =
        new()
        {
            {
                "Vienna", (48.2082, 16.3738)
            },
            {
                "Paris", (48.8566, 2.3522)
            },
            {
                "Berlin", (52.5200, 13.4050)
            },
            {
                "Budapest", (47.4979, 19.0402)
            },
            {
                "Warsaw", (52.2297, 21.0122)
            }
        };

    private readonly IJSRuntime _jsRuntime;
    private string _fromCity = string.Empty;

    private bool _isMapInitialized;
    private string _toCity = string.Empty;

    public MapViewModel(
        IJSRuntime jsRuntime,
        IHttpService httpService,
        IToastServiceWrapper toastServiceWrapper,
        ILogger logger
    )
        : base(httpService, toastServiceWrapper, logger)
    {
        _jsRuntime = jsRuntime;
        CityNames = new ObservableCollection<string>(GetCityNames());
    }

    public ObservableCollection<string> CityNames { get; }

    public string FromCity
    {
        get => _fromCity;
        set
        {
            if (!SetProperty(ref _fromCity, value))
            {
                return;
            }
            OnPropertyChanged(nameof(FilteredToCities));
            if (_toCity == _fromCity)
            {
                ToCity = string.Empty;
            }
        }
    }

    public string ToCity
    {
        get => _toCity;
        set => SetProperty(ref _toCity, value);
    }

    public IEnumerable<string> FilteredToCities => CityNames.Where(city => city != FromCity);

    public async Task InitializeMapAsync(ElementReference mapElement)
    {
        await _jsRuntime.InvokeVoidAsync("TourPlannerMap.initializeMap", mapElement);
        _isMapInitialized = true;
    }

    public Task ShowMapAsync() => ProcessAsync(async () =>
    {
        if (!_isMapInitialized)
        {
            ToastServiceWrapper.ShowError("Map is not initialized yet.");
            return;
        }

        if (string.IsNullOrWhiteSpace(FromCity) || string.IsNullOrWhiteSpace(ToCity))
        {
            ToastServiceWrapper.ShowError("Please select both cities.");
            return;
        }

        var fromCoords = GetCoordinates(FromCity);
        var toCoords = GetCoordinates(ToCity);

        if (fromCoords.HasValue && toCoords.HasValue)
        {
            await Task.Delay(500);
            await _jsRuntime.InvokeVoidAsync(
                "TourPlannerMap.setRoute",
                fromCoords.Value.Latitude,
                fromCoords.Value.Longitude,
                toCoords.Value.Latitude,
                toCoords.Value.Longitude
            );
        }
    });
    
    public async Task ClearMapAsync()
    {
        await _jsRuntime.InvokeVoidAsync("TourPlannerMap.clearMap");
        FromCity = string.Empty;
        ToCity = string.Empty;
        OnPropertyChanged(nameof(FromCity));
        OnPropertyChanged(nameof(ToCity));
    }

    public virtual (double Latitude, double Longitude)? GetCoordinates(string city) => Coordinates.TryGetValue(city, out var coords) ? coords : null;

    private static List<string> GetCityNames() => Coordinates.Keys.ToList();
}