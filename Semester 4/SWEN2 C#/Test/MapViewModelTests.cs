using Microsoft.JSInterop;
using Moq;
using UI.Service.Interface;
using UI.ViewModel;
using ILogger=Serilog.ILogger;

namespace Test;

[TestFixture]
public class MapViewModelTests
{
    [SetUp]
    public void Setup()
    {
        _mockJsRuntime = TestData.CreateMockJsRuntime();
        _mockHttpService = TestData.CreateMockHttpService();
        _mockToastService = TestData.CreateMockToastService();
        _mockLogger = TestData.CreateMockLogger();

        _viewModel = new MapViewModel(
        _mockJsRuntime.Object,
        _mockHttpService.Object,
        _mockToastService.Object,
        _mockLogger.Object
        );
    }

    private Mock<IJSRuntime> _mockJsRuntime;
    private Mock<IHttpService> _mockHttpService;
    private Mock<IToastServiceWrapper> _mockToastService;
    private Mock<ILogger> _mockLogger;
    private MapViewModel _viewModel;

    [Test]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        Assert.That(_viewModel.CityNames, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(_viewModel.CityNames, Has.Count.EqualTo(MapViewModel._coordinates.Count));
            Assert.That(_viewModel.FromCity, Is.Empty);
            Assert.That(_viewModel.ToCity, Is.Empty);
        });
    }

    [Test]
    public void FromCity_SetProperty_UpdatesFilteredToCities()
    {
        const string selectedCity = "Vienna";
        _viewModel.FromCity = selectedCity;

        Assert.Multiple(() => {
            Assert.That(_viewModel.FromCity, Is.EqualTo(selectedCity));
            Assert.That(_viewModel.FilteredToCities, Does.Not.Contain(selectedCity));
        });
        Assert.That(
        _viewModel.FilteredToCities.Count(),
        Is.EqualTo(_viewModel.CityNames.Count - 1)
        );
    }

    [Test]
    public void FromCity_SetSameAsToCity_ClearsToCity()
    {
        const string city = "Vienna";
        _viewModel.ToCity = city;
        _viewModel.FromCity = city;

        Assert.Multiple(() => {
            Assert.That(_viewModel.FromCity, Is.EqualTo(city));
            Assert.That(_viewModel.ToCity, Is.Empty);
        });
    }

    [Test]
    public void GetCoordinates_ValidCity_ReturnsCoordinates()
    {
        var result = _viewModel.GetCoordinates("Vienna");

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result.Value.Latitude, Is.EqualTo(48.2082));
            Assert.That(result.Value.Longitude, Is.EqualTo(16.3738));
        });
    }

    [Test]
    public void GetCoordinates_InvalidCity_ReturnsNull()
    {
        var result = _viewModel.GetCoordinates("InvalidCity");

        Assert.That(result, Is.Null);
    }
}
