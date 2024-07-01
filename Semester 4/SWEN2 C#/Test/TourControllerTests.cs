using API.Controllers;
using BL.DomainModel;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UI.Model;

namespace Test;

[TestFixture]
public class TourControllerTests
{
    [SetUp]
    public void Setup()
    {
        _mockTourService = new Mock<ITourService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new TourController(_mockTourService.Object, _mockMapper.Object);
    }

    private Mock<ITourService> _mockTourService;
    private Mock<IMapper> _mockMapper;
    private TourController _controller;

    [Test]
    public async Task CreateTour_HappyPath_ReturnsCreatedTour()
    {
        // Arrange
        var tourDto = TestData.CreateSampleTour();
        var tourDomain = TestData.CreateSampleTourDomain();
        _mockMapper.Setup(m => m.Map<TourDomain>(tourDto)).Returns(tourDomain);
        _mockTourService.Setup(s => s.CreateTourAsync(tourDomain)).ReturnsAsync(tourDomain);
        _mockMapper.Setup(m => m.Map<Tour>(tourDomain)).Returns(tourDto);

        // Act
        var result = await _controller.CreateTour(tourDto);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.EqualTo(tourDto));
    }

    [Test]
    public Task CreateTour_UnhappyPath_ValidationFails()
    {
        // Arrange
        var tourDto = TestData.CreateSampleTour();
        var tourDomain = TestData.CreateSampleTourDomain();
        _mockMapper.Setup(m => m.Map<TourDomain>(tourDto)).Returns(tourDomain);
        _mockTourService
            .Setup(s => s.CreateTourAsync(tourDomain))
            .ThrowsAsync(new ArgumentException("Invalid tour data"));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _controller.CreateTour(tourDto));
        return Task.CompletedTask;
    }

    [Test]
    public async Task GetAllTours_HappyPath_ReturnsAllTours()
    {
        // Arrange
        var toursDomain = TestData.CreateSampleTourDomainList();
        var toursDto = TestData.CreateSampleTourList();
        _mockTourService.Setup(s => s.GetAllToursAsync()).ReturnsAsync(toursDomain);
        _mockMapper.Setup(m => m.Map<IEnumerable<Tour>>(toursDomain)).Returns(toursDto);

        // Act
        var result = await _controller.GetAllTours();

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.EqualTo(toursDto));
    }

    [Test]
    public Task GetAllTours_UnhappyPath_DatabaseError()
    {
        // Arrange
        _mockTourService
            .Setup(s => s.GetAllToursAsync())
            .ThrowsAsync(new Exception("Database connection error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _controller.GetAllTours());
        return Task.CompletedTask;
    }

    [Test]
    public void GetTourById_HappyPath_ReturnsTour()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tourDomain = TestData.CreateSampleTourDomain();
        var tourDto = TestData.CreateSampleTour();
        _mockTourService.Setup(s => s.GetTourById(tourId)).Returns(tourDomain);
        _mockMapper.Setup(m => m.Map<Tour>(tourDomain)).Returns(tourDto);

        // Act
        var result = _controller.GetTourById(tourId);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.EqualTo(tourDto));
    }

    [Test]
    public void GetTourById_UnhappyPath_TourNotFound()
    {
        // Arrange
        var tourId = TestData.NonexistentGuid;
        _mockTourService
            .Setup(s => s.GetTourById(tourId))
            .Throws(new KeyNotFoundException("Tour not found"));

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _controller.GetTourById(tourId));
    }

    [Test]
    public async Task UpdateTour_HappyPath_ReturnsUpdatedTour()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tourDto = TestData.CreateSampleTour();
        tourDto.Id = tourId;
        var tourDomain = TestData.CreateSampleTourDomain();
        _mockMapper.Setup(m => m.Map<TourDomain>(tourDto)).Returns(tourDomain);
        _mockTourService.Setup(s => s.UpdateTourAsync(tourDomain)).ReturnsAsync(tourDomain);
        _mockMapper.Setup(m => m.Map<Tour>(tourDomain)).Returns(tourDto);

        // Act
        var result = await _controller.UpdateTour(tourId, tourDto);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.EqualTo(tourDto));
    }

    [Test]
    public async Task UpdateTour_UnhappyPath_IdMismatch()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tourDto = TestData.CreateSampleTour();
        tourDto.Id = Guid.NewGuid();// Different from tourId

        // Act
        var result = await _controller.UpdateTour(tourId, tourDto);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.That(badRequestResult.Value, Is.EqualTo("ID mismatch"));
    }

    [Test]
    public async Task DeleteTour_HappyPath_ReturnsNoContent()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        _mockTourService.Setup(s => s.DeleteTourAsync(tourId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTour(tourId);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public Task DeleteTour_UnhappyPath_TourNotFound()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        _mockTourService
            .Setup(s => s.DeleteTourAsync(tourId))
            .ThrowsAsync(new KeyNotFoundException("Tour not found"));

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.DeleteTour(tourId));
        return Task.CompletedTask;
    }

    [Test]
    public void SearchTours_HappyPath_ReturnsMatchingTours()
    {
        // Arrange
        const string searchText = TestData.ValidSearchText;
        var toursDomain = TestData.CreateSampleTourDomainList().AsQueryable();
        var toursDto = TestData.CreateSampleTourList();
        _mockTourService.Setup(s => s.SearchTours(searchText)).Returns(toursDomain);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<Tour>>(It.IsAny<IEnumerable<TourDomain>>()))
            .Returns(toursDto);

        // Act
        var result = _controller.SearchTours(searchText);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(toursDto));
    }

    [Test]
    public void SearchTours_UnhappyPath_NoMatchingTours()
    {
        // Arrange
        const string searchText = TestData.InvalidSearchText;
        _mockTourService
            .Setup(s => s.SearchTours(searchText))
            .Returns(new List<TourDomain>().AsQueryable());
        _mockMapper
            .Setup(m => m.Map<IEnumerable<Tour>>(It.IsAny<IEnumerable<TourDomain>>()))
            .Returns(new List<Tour>());

        // Act
        var result = _controller.SearchTours(searchText);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(((IEnumerable<Tour>)okResult.Value!).Count(), Is.EqualTo(0));
    }
}
