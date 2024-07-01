using System.Collections.ObjectModel;
using Microsoft.JSInterop;
using UI.Decorator;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.ViewModel;

public class TourLogViewModel : BaseViewModel
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IViewModelHelperService _viewModelHelperService;
    private bool _isEditing;
    private bool _isLogFormVisible;
    private Guid? _selectedTourId;
    private TourLog _selectedTourLog = new();

    public TourLogViewModel(
        IHttpService httpService,
        IToastServiceWrapper toastServiceWrapper,
        IJSRuntime jsRuntime,
        ILogger logger,
        IViewModelHelperService viewModelHelperService
    )
        : base(httpService, toastServiceWrapper, logger)
    {
        _viewModelHelperService = viewModelHelperService;
        TourLogs = new ObservableCollection<TourLog>();
        _jsRuntime = jsRuntime;
    }

    public ObservableCollection<TourLog> TourLogs { get; }

    public TourLog SelectedTourLog
    {
        get => _selectedTourLog;
        set => SetProperty(ref _selectedTourLog, value);
    }

    public bool IsLogFormVisible
    {
        get => _isLogFormVisible;
        set => SetProperty(ref _isLogFormVisible, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public Guid? SelectedTourId
    {
        get => _selectedTourId;
        set
        {
            if (SetProperty(ref _selectedTourId, value))
            {
                _ = HandleTourSelection();
            }
        }
    }

    public bool IsFormValid
    {
        get => !string.IsNullOrWhiteSpace(SelectedTourLog.Comment) &&
               SelectedTourLog.Difficulty is >= 1 and <= 5 &&
               SelectedTourLog is { TotalDistance: > 0, TotalTime: > 0, Rating: >= 1 and <= 5 };
    }

    private Task HandleTourSelection() => HandleApiRequestAsync(
    async () => {
        if (_selectedTourId == null || _selectedTourId == Guid.Empty)
        {
            ClearTourData();
        }
        else
        {
            await LoadTourLogsAsync();
        }
    },
    "Error handling tour selection"
    );

    private void ClearTourData()
    {
        TourLogs.Clear();
        ResetForm();
    }

    private void ShowAddLogForm()
    {
        if (!SelectedTourId.HasValue)
        {
            return;
        }

        _viewModelHelperService.ResetForm(
        ref _selectedTourLog,
        () => new TourLog
        {
            TourId = SelectedTourId.Value
        }
        );
        _viewModelHelperService.ShowForm(ref _isLogFormVisible);
        IsEditing = false;
        OnPropertyChanged(nameof(SelectedTourLog));
        OnPropertyChanged(nameof(IsLogFormVisible));
        OnPropertyChanged(nameof(IsEditing));
    }

    public void ResetForm()
    {
        _viewModelHelperService.ResetForm(
        ref _selectedTourLog,
        () => new TourLog
        {
            TourId = SelectedTourId ?? Guid.Empty
        }
        );
        IsLogFormVisible = false;
        IsEditing = false;
        OnPropertyChanged(nameof(SelectedTourLog));
        OnPropertyChanged(nameof(IsLogFormVisible));
        OnPropertyChanged(nameof(IsEditing));
    }

    public void ToggleLogForm()
    {
        if (IsLogFormVisible)
        {
            ResetForm();
        }
        else
        {
            ShowAddLogForm();
        }
    }

    [UiMethodDecorator]
    public Task LoadTourLogsAsync() => HandleApiRequestAsync(
    async () => {
        if (!SelectedTourId.HasValue)
        {
            return;
        }

        var logs = await HttpService.GetListAsync<TourLog>(
        $"api/tourlog/bytour/{SelectedTourId}"
        );
        TourLogs.Clear();
        foreach (var log in logs ?? Array.Empty<TourLog>())
        {
            TourLogs.Add(log);
        }
        ResetForm();
    },
    "Error loading tour logs"
    );

    [UiMethodDecorator]
    public Task<bool> SaveTourLogAsync() => HandleApiRequestAsync(
    async () => {
        if (!IsFormValid || !SelectedTourId.HasValue)
        {
            ToastServiceWrapper.ShowError("Not Valid.");
            return false;
        }

        if (SelectedTourLog.Id == Guid.Empty)
        {
            await HttpService.PostAsync<TourLog>("api/tourlog", SelectedTourLog);
            ToastServiceWrapper.ShowSuccess("Tour log created successfully.");
        }
        else
        {
            await HttpService.PutAsync<TourLog>(
            $"api/tourlog/{SelectedTourLog.Id}",
            SelectedTourLog
            );
            ToastServiceWrapper.ShowSuccess("Tour log updated successfully.");
        }

        await LoadTourLogsAsync();
        ResetForm();
        return true;
    },
    "Error saving tour log"
    );

    [UiMethodDecorator]
    private Task EditTourLogAsync(Guid logId) => HandleApiRequestAsync(
    async () => {
        var log = await HttpService.GetAsync<TourLog>($"api/tourlog/{logId}");
        if (log != null)
        {
            SelectedTourLog = log;
            SelectedTourId = log.TourId;
            IsLogFormVisible = true;
            IsEditing = true;
        }
    },
    "Error loading tour log for editing"
    );

    [UiMethodDecorator]
    public Task EditHandleTourLogAction(Guid? logId = null) => HandleApiRequestAsync(
    async () => {
        if (logId.HasValue && logId != Guid.Empty)
        {
            if (IsEditing && IsLogFormVisible && SelectedTourLog.Id == logId)
            {
                ResetForm();
            }
            else
            {
                await EditTourLogAsync(logId.Value);
            }
        }
        else
        {
            ToggleLogForm();
        }
    },
    "Error handling tour log action"
    );

    [UiMethodDecorator]
    public async Task DeleteTourLogAsync(Guid logId)
    {
        var confirmed = await _jsRuntime.InvokeAsync<bool>(
        "confirm",
        "Are you sure you want to delete this tour log?"
        );
        if (confirmed)
        {
            await HandleApiRequestAsync(
            async () => {
                if (logId == Guid.Empty)
                {
                    return;
                }
                await HttpService.DeleteAsync($"api/tourlog/{logId}");
                await LoadTourLogsAsync();
                ToastServiceWrapper.ShowSuccess("Tour log deleted successfully.");
            },
            "Error deleting tour log"
            );
        }
    }
}
