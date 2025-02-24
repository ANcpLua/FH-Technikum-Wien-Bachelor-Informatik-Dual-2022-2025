using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Moq;
using Serilog;
using UI.Model;
using UI.Service.Interface;
using UI.ViewModel;

namespace Test.UI;

[TestFixture]
public class TourViewModelTests
{
    private Mock<IHttpService> _mockHttpService;
    private Mock<IToastServiceWrapper> _mockToastService;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IJSRuntime> _mockJsRuntime;
    private Mock<IRouteApiService> _mockRouteApiService;
    private Mock<ILogger> _mockLogger;
    private Mock<IViewModelHelperService> _mockViewModelHelperService;
    private Mock<MapViewModel> _mockMapViewModel;
    private TourViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockHttpService = TestData.CreateMockHttpService();
        _mockToastService = TestData.CreateMockToastService();
        _mockConfiguration = TestData.CreateMockConfiguration();
        _mockJsRuntime = TestData.CreateMockJsRuntime();
        _mockRouteApiService = TestData.CreateMockRouteApiService();
        _mockLogger = TestData.CreateMockLogger();
        _mockMapViewModel = TestData.CreateMockMapViewModel();
        _mockViewModelHelperService = TestData.CreateMockViewModelHelperService();

        _viewModel = new TourViewModel(
            _mockHttpService.Object,
            _mockToastService.Object,
            _mockConfiguration.Object,
            _mockJsRuntime.Object,
            _mockRouteApiService.Object,
            _mockLogger.Object,
            _mockMapViewModel.Object,
            _mockViewModelHelperService.Object
        );
    }

    [Test]
    public async Task LoadToursAsync_LoadsToursSuccessfully()
    {
        // Act
        await _viewModel.LoadToursAsync();

        // Assert
        Assert.That(_viewModel.Tours, Is.Not.Empty);
        Assert.That(_viewModel.Tours, Has.Count.EqualTo(5));
    }

    [Test]
    public async Task LoadToursAsync_ShowsErrorMessageOnFailure()
    {
        // Arrange
        _mockHttpService
            .Setup(s => s.GetListAsync<Tour>(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Network error"));

        // Act
        await _viewModel.LoadToursAsync();

        // Assert
        _mockToastService.Verify(
            t => t.ShowError(It.Is<string>(msg => msg.Contains("Error loading tours"))),
            Times.Once
        );
    }

    [Test]
    public async Task DeleteTourAsync_DoesNotDeleteTourOnCancel()
    {
        // Arrange
        var testTour = TestData.CreateSampleTour();
        _mockJsRuntime
            .Setup(js => js.InvokeAsync<bool>("confirm", It.IsAny<object[]>()))
            .ReturnsAsync(false);

        _viewModel.Tours.Add(testTour);

        // Act
        await _viewModel.DeleteTourAsync(testTour.Id);

        // Assert
        Assert.That(_viewModel.Tours, Is.Not.Empty);
        _mockHttpService.Verify(s => s.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task ShowTourDetailsAsync_LoadsTourSuccessfully()
    {
        // Arrange
        var testTour = TestData.CreateSampleTour();
        _mockHttpService.Setup(s => s.GetAsync<Tour>(It.IsAny<string>())).ReturnsAsync(testTour);

        // Act
        await _viewModel.ShowTourDetailsAsync(testTour.Id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_viewModel.ModalTour.Id, Is.EqualTo(testTour.Id));
            Assert.That(_viewModel.ModalTour.Name, Is.EqualTo(testTour.Name));
            Assert.That(_viewModel.ModalTour.Description, Is.EqualTo(testTour.Description));
        });
        _mockHttpService.Verify(s => s.GetAsync<Tour>($"api/tour/{testTour.Id}"), Times.Once);
    }

    [Test]
    public async Task SaveTourAsync_ShowsErrorMessageOnFailure()
    {
        // Arrange
        var testTour = TestData.CreateSampleTourWithVariousProperties();
        _mockHttpService
            .Setup(s => s.PostAsync<Tour>(It.IsAny<string>(), It.IsAny<object>()))
            .ThrowsAsync(new Exception("Network error"));

        _viewModel.SelectedTour = testTour;

        // Act
        var result = await _viewModel.SaveTourAsync();

        // Assert
        Assert.That(result, Is.False);
        _mockToastService.Verify(
            t => t.ShowError(It.Is<string>(msg => msg.Contains("Error saving tour"))),
            Times.Once
        );
    }

    [Test]
    public async Task ShowTourDetailsAsync_ShowsErrorMessageOnFailure()
    {
        // Arrange
        _mockHttpService
            .Setup(s => s.GetAsync<Tour>(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Network error"));

        // Act
        await _viewModel.ShowTourDetailsAsync(TestData.TestGuid);

        // Assert
        _mockToastService.Verify(
            t => t.ShowError(It.Is<string>(msg => msg.Contains("Error loading tour details"))),
            Times.Once
        );
    }

    [Test]
    public async Task EditTourAsync_LoadsTourSuccessfully()
    {
        // Arrange
        var testTour = TestData.CreateSampleTour();
        _mockHttpService.Setup(s => s.GetAsync<Tour>(It.IsAny<string>())).ReturnsAsync(testTour);

        // Act
        await _viewModel.EditTourAsync(testTour.Id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_viewModel.SelectedTour.Id, Is.EqualTo(testTour.Id));
            Assert.That(_viewModel.SelectedTour.Name, Is.EqualTo(testTour.Name));
            Assert.That(_viewModel.SelectedTour.Description, Is.EqualTo(testTour.Description));
        });
        _mockHttpService.Verify(s => s.GetAsync<Tour>($"api/tour/{testTour.Id}"), Times.Once);
    }

    [Test]
    public async Task EditTourAsync_ShowsErrorMessageOnFailure()
    {
        // Arrange
        _mockHttpService
            .Setup(s => s.GetAsync<Tour>(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Network error"));

        // Act
        await _viewModel.EditTourAsync(Guid.Empty);

        // Assert
        _mockToastService.Verify(
            t => t.ShowError(It.Is<string>(msg => msg.Contains("Error handling tour edit action"))),
            Times.Once
        );
    }

    [Test]
    public async Task SaveTourAsync_ExistingTour_SavesSuccessfully()
    {
        // Arrange
        var testTour = TestData.CreateSampleTourWithVariousProperties();
        var fromCoords = TestData.CreateSampleCoordinates();
        var toCoords = TestData.CreateSampleCoordinates();

        TestData.SetupMapViewModel(_mockMapViewModel, fromCoords, toCoords);
        TestData.SetupRouteApiService(_mockRouteApiService, fromCoords, toCoords);
        TestData.SetupHttpServicePut(_mockHttpService, testTour);

        _viewModel.SelectedTour = testTour;

        // Act
        var result = await _viewModel.SaveTourAsync();

        // Assert
        Assert.That(result, Is.True);
        _mockHttpService.Verify(
            s => s.PutAsync<Tour>($"api/tour/{testTour.Id}", It.IsAny<Tour>()),
            Times.Once
        );
        _mockToastService.Verify(
            t => t.ShowSuccess(It.Is<string>(msg => msg.Contains("Tour updated successfully"))),
            Times.Once
        );
    }

    [Test]
    public async Task SaveTourAsync_NewTour_SavesSuccessfully()
    {
        // Arrange
        var testTour = TestData.CreateSampleTourWithVariousProperties();
        testTour.Id = Guid.Empty;
        var fromCoords = TestData.CreateSampleCoordinates();
        var toCoords = TestData.CreateSampleCoordinates();

        TestData.SetupMapViewModel(_mockMapViewModel, fromCoords, toCoords);
        TestData.SetupRouteApiService(_mockRouteApiService, fromCoords, toCoords);
        _mockHttpService
            .Setup(s => s.PostAsync<Tour>("api/tour", It.IsAny<Tour>()))
            .ReturnsAsync(testTour);

        _viewModel.SelectedTour = testTour;

        // Act
        var result = await _viewModel.SaveTourAsync();

        // Assert
        Assert.That(result, Is.True);
        _mockHttpService.Verify(
            s => s.PostAsync<Tour>("api/tour", It.Is<Tour>(t => t.Id == Guid.Empty)),
            Times.Once
        );
        _mockToastService.Verify(
            t => t.ShowSuccess(It.Is<string>(msg => msg.Contains("Tour saved successfully"))),
            Times.Once
        );
    }

    [Test]
    public async Task DeleteTourAsync_Confirmed_DeletesTourAndShowsSuccess()
    {
        // Arrange
        var tourId = TestData.TestGuid;
        _mockJsRuntime
            .Setup(js => js.InvokeAsync<bool>("confirm", It.IsAny<object[]>()))
            .ReturnsAsync(true);

        // Act
        await _viewModel.DeleteTourAsync(tourId);

        // Assert
        Assert.That(_viewModel.Tours, Has.No.Member(tourId));
        _mockHttpService.Verify(s => s.DeleteAsync($"api/tour/{tourId}"), Times.Once);
        _mockToastService.Verify(t => t.ShowSuccess("Tour deleted successfully."), Times.Once);
    }
}