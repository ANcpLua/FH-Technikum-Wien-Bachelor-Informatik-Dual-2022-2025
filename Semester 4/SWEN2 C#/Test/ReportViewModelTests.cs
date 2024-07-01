using System.Collections.ObjectModel;
using System.Text;
using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel;
using ILogger=Serilog.ILogger;

namespace Test;

[TestFixture]
public class ReportViewModelTests
{
    [SetUp]
    public void Setup()
    {
        _mockHttpService = TestData.CreateMockHttpService();
        _mockToastService = TestData.CreateMockToastService();
        _mockViewModelHelper = TestData.CreateMockViewModelHelperService();
        _mockBlazorDownloadFile = TestData.CreateMockBlazorDownloadFileService();
        _mockLogger = TestData.CreateMockLogger();
        _mockTourViewModel = new Mock<TourViewModel>(
        TestData.CreateMockHttpService().Object,
        TestData.CreateMockToastService().Object,
        TestData.CreateMockConfiguration().Object,
        TestData.CreateMockJsRuntime().Object,
        TestData.CreateMockRouteApiService().Object,
        TestData.CreateMockLogger().Object,
        TestData.CreateMockMapViewModel().Object,
        TestData.CreateMockViewModelHelperService().Object
        )
        {
            CallBase = true
        };

        _viewModel = new ReportViewModel(
        _mockHttpService.Object,
        _mockToastService.Object,
        _mockLogger.Object,
        _mockBlazorDownloadFile.Object,
        _mockTourViewModel.Object,
        _mockViewModelHelper.Object
        );
    }

    private Mock<IHttpService> _mockHttpService;
    private Mock<IToastServiceWrapper> _mockToastService;
    private Mock<IBlazorDownloadFileService> _mockBlazorDownloadFile;
    private Mock<IViewModelHelperService> _mockViewModelHelper;
    private Mock<TourViewModel> _mockTourViewModel;
    private Mock<ILogger> _mockLogger;
    private ReportViewModel _viewModel;

    [Test]
    public async Task GenerateSummaryReportAsync_GeneratesAndDownloadsReport()
    {
        byte[] reportBytes =
        {
            1, 2, 3
        };
        _mockHttpService
            .Setup(s => s.GetByteArrayAsync("api/reports/summary"))
            .ReturnsAsync(reportBytes);

        await _viewModel.GenerateSummaryReportAsync();

        _mockBlazorDownloadFile.Verify(
        b =>
            b.DownloadFile(
            It.IsRegex(@"SummaryReport_\d{8}_\d{6}\.pdf"),
            reportBytes,
            "application/pdf"
            ),
        Times.Once
        );
        _mockToastService.Verify(
        t => t.ShowSuccess("SummaryReport generated successfully."),
        Times.Once
        );
    }

    [Test]
    public async Task GenerateDetailedReportAsync_WithValidTourId_GeneratesAndDownloadsReport()
    {
        var tourId = Guid.NewGuid();
        _viewModel.SelectedDetailedTourId = tourId;
        byte[] reportBytes =
        {
            1, 2, 3
        };
        _mockHttpService
            .Setup(s => s.GetByteArrayAsync($"api/reports/tour/{tourId}"))
            .ReturnsAsync(reportBytes);

        await _viewModel.GenerateDetailedReportAsync();

        _mockBlazorDownloadFile.Verify(
        b =>
            b.DownloadFile(
            It.IsRegex(@"DetailedReport_\d{8}_\d{6}\.pdf"),
            reportBytes,
            "application/pdf"
            ),
        Times.Once
        );
        _mockToastService.Verify(
        t => t.ShowSuccess("DetailedReport generated successfully."),
        Times.Once
        );
    }

    [Test]
    public async Task ExportTourToJsonAsync_WithValidTourId_ExportsTourAsJson()
    {
        var tourId = Guid.NewGuid();
        var jsonContent = TestData.CreateSampleTourJson();
        _mockHttpService
            .Setup(s => s.GetStringAsync($"api/reports/export/{tourId}"))
            .ReturnsAsync(jsonContent);

        await _viewModel.ExportTourToJsonAsync(tourId);

        _mockBlazorDownloadFile.Verify(
        b =>
            b.DownloadFile(
            It.IsRegex($@"Tour_{tourId}_\d{{8}}_\d{{6}}\.json"),
            It.Is<byte[]>(bytes => Encoding.UTF8.GetString(bytes) == jsonContent),
            "application/json"
            ),
        Times.Once
        );
        _mockToastService.Verify(t => t.ShowSuccess("Tour exported successfully."), Times.Once);
    }

    [Test]
    public async Task ExportTourToJsonAsync_WithInvalidTourData_ShowsErrorToast()
    {
        var tourId = Guid.NewGuid();
        _mockHttpService
            .Setup(s => s.GetStringAsync($"api/reports/export/{tourId}"))
            .ReturnsAsync(string.Empty);

        await _viewModel.ExportTourToJsonAsync(tourId);

        _mockToastService.Verify(
        t => t.ShowError("Error exporting tour: Invalid tour data."),
        Times.Once
        );
        _mockBlazorDownloadFile.Verify(
        b => b.DownloadFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()),
        Times.Never
        );
    }

    [Test]
    public async Task ImportTourFromJsonAsync_WithInvalidJson_ShowsErrorToast()
    {
        var invalidJson = "{invalid json}";
        var mockFile = TestData.CreateMockBrowserFile(content: invalidJson);
        mockFile
            .Setup(f => f.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(invalidJson)));
        var inputFileChangeEventArgs = new InputFileChangeEventArgs(new[]
        {
            mockFile.Object
        });

        await _viewModel.ImportTourFromJsonAsync(inputFileChangeEventArgs);

        _mockToastService.Verify(
        t => t.ShowError(It.Is<string>(s => s.StartsWith("Error importing tour"))),
        Times.Once
        );
        _mockHttpService.Verify(
        s => s.PostAsync(It.IsAny<string>(), It.IsAny<Tour>()),
        Times.Never
        );
    }

    [Test]
    public async Task ImportTourFromJsonAsync_WithValidJson_ImportsTourSuccessfully()
    {
        var jsonContent = TestData.CreateSampleTourJson();
        var mockFile = TestData.CreateMockBrowserFile(content: jsonContent);
        mockFile
            .Setup(f => f.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)));
        var inputFileChangeEventArgs = new InputFileChangeEventArgs(new[]
        {
            mockFile.Object
        });
        _mockHttpService
            .Setup(s => s.GetListAsync<Tour>("api/tour"))
            .ReturnsAsync(new ObservableCollection<Tour>());

        await _viewModel.ImportTourFromJsonAsync(inputFileChangeEventArgs);

        _mockToastService.Verify(t => t.ShowSuccess("Tour imported successfully."), Times.Once);
        _mockHttpService.Verify(s => s.PostAsync("api/tour", It.IsAny<Tour>()), Times.Once);
    }

    [Test]
    public async Task ExportTourToJsonAsync_ExportsTourSuccessfully()
    {
        var tourId = Guid.NewGuid();
        var jsonContent = TestData.CreateSampleTourJson();
        _mockHttpService
            .Setup(s => s.GetStringAsync($"api/reports/export/{tourId}"))
            .ReturnsAsync(jsonContent);

        await _viewModel.ExportTourToJsonAsync(tourId);

        _mockHttpService.Verify(s => s.GetStringAsync($"api/reports/export/{tourId}"), Times.Once);
        _mockBlazorDownloadFile.Verify(
        b =>
            b.DownloadFile(
            It.Is<string>(f => f.StartsWith("Tour_")),
            It.Is<byte[]>(bytes => Encoding.UTF8.GetString(bytes) == jsonContent),
            "application/json"
            ),
        Times.Once
        );
        _mockToastService.Verify(t => t.ShowSuccess("Tour exported successfully."), Times.Once);
    }

    [Test]
    public void InitializeViewModel_CreatesReportViewModel()
    {
        var viewModel = new ReportViewModel(
        _mockHttpService.Object,
        _mockToastService.Object,
        _mockLogger.Object,
        _mockBlazorDownloadFile.Object,
        _mockTourViewModel.Object,
        _mockViewModelHelper.Object
        );

        Assert.That(viewModel, Is.Not.Null);
    }
}
