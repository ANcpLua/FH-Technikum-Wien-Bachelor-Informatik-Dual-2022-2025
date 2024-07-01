using System.Text;
using System.Text.Json;
using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Forms;
using UI.Decorator;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.ViewModel;

public class ReportViewModel : BaseViewModel
{
    private readonly IBlazorDownloadFileService _blazorDownloadFile;
    private readonly TourViewModel _tourViewModel;
    private readonly IViewModelHelperService _viewModelHelper;
    private string _currentReportUrl = string.Empty;
    private Guid _selectedDetailedTourId = Guid.Empty;

    public ReportViewModel(
        IHttpService httpService,
        IToastServiceWrapper toastServiceWrapper,
        ILogger logger,
        IBlazorDownloadFileService blazorDownloadFile,
        TourViewModel tourViewModel,
        IViewModelHelperService viewModelHelper
    )
        : base(httpService, toastServiceWrapper, logger)
    {
        _blazorDownloadFile = blazorDownloadFile;
        _tourViewModel = tourViewModel;
        _viewModelHelper = viewModelHelper;
    }

    public string CurrentReportUrl
    {
        get => _currentReportUrl;
        set => SetProperty(ref _currentReportUrl, value);
    }

    public Guid SelectedDetailedTourId
    {
        get => _selectedDetailedTourId;
        set => SetProperty(ref _selectedDetailedTourId, value);
    }

    public IEnumerable<Tour> Tours
    {
        get => _tourViewModel.Tours;
    }

    [UiMethodDecorator]
    public Task InitializeAsync() => _tourViewModel.LoadToursAsync();

    private void ResetCurrentReportUrl()
    {
        _viewModelHelper.ResetForm(ref _currentReportUrl, () => string.Empty);
        OnPropertyChanged(nameof(CurrentReportUrl));
    }

    public void ClearCurrentReport() => ResetCurrentReportUrl();

    [UiMethodDecorator]
    public Task GenerateDetailedReportAsync() => HandleApiRequestAsync(
    async () => {
        if (SelectedDetailedTourId == Guid.Empty)
        {
            return;
        }

        var tour = Tours.FirstOrDefault(t => t.Id == SelectedDetailedTourId);
        if (tour != null && !string.IsNullOrEmpty(tour.ImagePath))
        {
            tour.ImagePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            tour.ImagePath
            );
        }

        await GenerateAndDownloadReport(
        $"api/reports/tour/{SelectedDetailedTourId}",
        "DetailedReport"
        );
    },
    "Error generating detailed report"
    );

    [UiMethodDecorator]
    public Task GenerateSummaryReportAsync() => HandleApiRequestAsync(
    async () => {
        await GenerateAndDownloadReport("api/reports/summary", "SummaryReport");
    },
    "Error generating summary report"
    );

    [UiMethodDecorator]
    private async Task GenerateAndDownloadReport(string uri, string reportType)
    {
        var reportBytes = await HttpService.GetByteArrayAsync(uri);
        var fileName = $"{reportType}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
        await _blazorDownloadFile.DownloadFile(fileName, reportBytes, "application/pdf");

        if (reportBytes != null)
        {
            _viewModelHelper.ResetForm(
            ref _currentReportUrl,
            () => $"data:application/pdf;base64,{Convert.ToBase64String(reportBytes)}"
            );
            OnPropertyChanged(nameof(CurrentReportUrl));
        }
        ToastServiceWrapper.ShowSuccess($"{reportType} generated successfully.");
    }

    [UiMethodDecorator]
    public Task ExportTourToJsonAsync(Guid tourId) => HandleApiRequestAsync(
    async () => {
        var json = await HttpService.GetStringAsync($"api/reports/export/{tourId}");
        {
            if (string.IsNullOrEmpty(json))
            {
                ToastServiceWrapper.ShowError("Error exporting tour: Invalid tour data.");
                return;
            }
        }

        var fileName = $"Tour_{tourId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
        await _blazorDownloadFile.DownloadFile(
        fileName,
        Encoding.UTF8.GetBytes(json),
        "application/json"
        );

        ToastServiceWrapper.ShowSuccess("Tour exported successfully.");
    },
    "Error exporting tour"
    );

    [UiMethodDecorator]
    public Task ImportTourFromJsonAsync(InputFileChangeEventArgs e) => HandleApiRequestAsync(
    async () => {
        await using var stream = e.File.OpenReadStream();
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var tour = JsonSerializer.Deserialize<Tour>(
        json,
        new JsonSerializerOptions
        {
            WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }
        );

        if (tour is null)
        {
            ToastServiceWrapper.ShowError("Error importing tour: Invalid tour data.");
            return;
        }

        var existingTour = Tours.FirstOrDefault(t => t.Id == tour.Id);
        if (existingTour is not null)
        {
            ToastServiceWrapper.ShowError(
            $"Tour already exists. Delete {existingTour.Name} first."
            );
            return;
        }

        await HttpService.PostAsync("api/tour", tour);
        await _tourViewModel.LoadToursAsync();
        ToastServiceWrapper.ShowSuccess("Tour imported successfully.");
    },
    "Error importing tour"
    );
}
