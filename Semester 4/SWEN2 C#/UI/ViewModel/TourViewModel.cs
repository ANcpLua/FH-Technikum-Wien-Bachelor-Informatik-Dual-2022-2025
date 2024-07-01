using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.JSInterop;
using UI.Decorator;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.ViewModel;

public class TourViewModel : BaseViewModel
{
    private readonly IConfiguration _configuration;
    private readonly IJSRuntime _jsRuntime;
    private readonly MapViewModel _mapViewModel;
    private readonly IRouteApiService _routeApiService;
    private readonly IViewModelHelperService _viewModelHelperService;

    private bool _isFormVisible;
    private bool _isMapVisible;
    private Tour _modalTour = new();
    private Tour _selectedTour = new();

    public TourViewModel(
        IHttpService httpService,
        IToastServiceWrapper toastServiceWrapper,
        IConfiguration configuration,
        IJSRuntime jsRuntime,
        IRouteApiService routeApiService,
        ILogger logger,
        MapViewModel mapViewModel,
        IViewModelHelperService viewModelHelperService
    )
        : base(httpService, toastServiceWrapper, logger)
    {
        _configuration = configuration;
        _jsRuntime = jsRuntime;
        _mapViewModel = mapViewModel;
        _routeApiService = routeApiService;
        _viewModelHelperService = viewModelHelperService;

        Tours = new ObservableCollection<Tour>();
    }

    public ObservableCollection<Tour> Tours { get; set; }

    public bool IsFormVisible
    {
        get => _isFormVisible;
        set => SetProperty(ref _isFormVisible, value);
    }

    public Tour SelectedTour
    {
        get => _selectedTour;
        set
        {
            if (!SetProperty(ref _selectedTour, value))
            {
                return;
            }
            _mapViewModel.FromCity = value.From;
            _mapViewModel.ToCity = value.To;
            OnPropertyChanged(nameof(FilteredToCities));
        }
    }

    public Tour ModalTour
    {
        get => _modalTour;
        private set => SetProperty(ref _modalTour, value);
    }

    public bool IsMapVisible
    {
        get => _isMapVisible;
        set => SetProperty(ref _isMapVisible, value);
    }

    public bool IsFormValid
    {
        get => !string.IsNullOrWhiteSpace(SelectedTour.Name) &&
               !string.IsNullOrWhiteSpace(SelectedTour.Description) &&
               !string.IsNullOrWhiteSpace(SelectedTour.From) &&
               !string.IsNullOrWhiteSpace(SelectedTour.To) &&
               !string.IsNullOrWhiteSpace(SelectedTour.TransportType);
    }

    public IEnumerable<string> FilteredToCities
    {
        get => _mapViewModel.CityNames.Where(city => city != SelectedTour.From);
    }

    public void ToggleMap()
    {
        _viewModelHelperService.ToggleVisibility(ref _isMapVisible);
        OnPropertyChanged(nameof(IsMapVisible));
    }

    public void ShowAddTourForm()
    {
        if (IsFormVisible)
        {
            ResetForm();
        }
        else
        {
            SelectedTour = new Tour();
            _mapViewModel.FromCity = string.Empty;
            _mapViewModel.ToCity = string.Empty;
            _viewModelHelperService.ShowForm(ref _isFormVisible);
            OnPropertyChanged(nameof(IsFormVisible));
        }
    }

    private void ResetForm()
    {
        _viewModelHelperService.ResetForm(ref _selectedTour, () => new Tour());
        _mapViewModel.FromCity = string.Empty;
        _mapViewModel.ToCity = string.Empty;
        IsFormVisible = false;
        OnPropertyChanged(nameof(SelectedTour));
        OnPropertyChanged(nameof(IsFormVisible));
    }

    [UiMethodDecorator]
    public Task LoadToursAsync() => HandleApiRequestAsync(
    async () => {
        var tours = await HttpService.GetListAsync<Tour>("api/tour");
        Tours = new ObservableCollection<Tour>(tours ?? Array.Empty<Tour>());
        OnPropertyChanged(nameof(Tours));
    },
    "Error loading tours"
    );

    [UiMethodDecorator]
    public Task<bool> SaveTourAsync() => HandleApiRequestAsync(
    async () => {
        if (!IsFormValid)
        {
            ToastServiceWrapper.ShowError("Please fill in all required fields correctly.");
            return false;
        }

        var fromCoords = _mapViewModel.GetCoordinates(SelectedTour.From);
        var toCoords = _mapViewModel.GetCoordinates(SelectedTour.To);

        try
        {
            var routeData = await _routeApiService.FetchRouteDataAsync(
            fromCoords!.Value,
            toCoords!.Value,
            SelectedTour.TransportType
            );

            SelectedTour.Distance = routeData.Distance;
            SelectedTour.EstimatedTime = routeData.Duration;
            SelectedTour.ImagePath =
                $"{_configuration["AppSettings:ImageBasePath"]}{SelectedTour.From}{SelectedTour.To}.png";

            var routeInformation = new
            {
                FromCoordinates = new
                {
                    fromCoords.Value.Latitude, fromCoords.Value.Longitude
                },
                ToCoordinates = new
                {
                    toCoords.Value.Latitude, toCoords.Value.Longitude
                },
                routeData.Distance,
                routeData.Duration
            };

            SelectedTour.RouteInformation = JsonSerializer.Serialize(routeInformation);

            if (SelectedTour.Id == Guid.Empty)
            {
                await HttpService.PostAsync<Tour>("api/tour", SelectedTour);
                ToastServiceWrapper.ShowSuccess("Tour saved successfully.");
            }
            else
            {
                await HttpService.PutAsync<Tour>(
                $"api/tour/{SelectedTour.Id}",
                SelectedTour
                );
                ToastServiceWrapper.ShowSuccess("Tour updated successfully.");
            }

            await LoadToursAsync();

            await Task.Delay(500);

            IsMapVisible = true;
            OnPropertyChanged(nameof(IsMapVisible));

            await Task.Delay(100);

            await _jsRuntime.InvokeVoidAsync(
            "TourPlannerMap.setRoute",
            fromCoords.Value.Latitude,
            fromCoords.Value.Longitude,
            toCoords.Value.Latitude,
            toCoords.Value.Longitude
            );

            ResetForm();

            return true;
        }
        catch (Exception ex)
        {
            ToastServiceWrapper.ShowError($"Error saving tour: {ex.Message}");
            return false;
        }
    },
    "Error saving tour"
    );

    [UiMethodDecorator]
    public Task ShowTourDetailsAsync(Guid id) => HandleApiRequestAsync(
    async () => {
        ModalTour = await HttpService.GetAsync<Tour>($"api/tour/{id}") ?? new Tour();
        await _jsRuntime.InvokeVoidAsync("showModal", "tourDetailsModal");
    },
    "Error loading tour details"
    );

    [UiMethodDecorator]
    public Task EditTourAsync(Guid id) => HandleApiRequestAsync(
    async () => {
        if (IsFormVisible && SelectedTour.Id == id)
        {
            ResetForm();
        }
        else
        {
            SelectedTour = await HttpService.GetAsync<Tour>($"api/tour/{id}") ?? new Tour();
            IsFormVisible = true;
        }
    },
    "Error handling tour edit action"
    );

    [UiMethodDecorator]
    public async Task DeleteTourAsync(Guid id)
    {
        var confirmed = await _jsRuntime.InvokeAsync<bool>(
        "confirm",
        "Are you sure you want to delete this tour?"
        );
        if (confirmed)
        {
            await HandleApiRequestAsync(
            async () => {
                await HttpService.DeleteAsync($"api/tour/{id}");
                await LoadToursAsync();
                ToastServiceWrapper.ShowSuccess("Tour deleted successfully.");
            },
            "Error deleting tour"
            );
        }
    }
}
