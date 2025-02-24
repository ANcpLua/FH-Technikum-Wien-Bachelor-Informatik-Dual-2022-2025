using API.Controllers;
using BL.DomainModel;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UI.Model;

namespace Test.API;

[TestFixture]
public class TourLogControllerTests
{
    private Mock<ITourLogService> _mockTourLogService;
    private Mock<IMapper> _mockMapper;
    private TourLogController _controller;

    [SetUp]
    public void Setup()
    {
        _mockTourLogService = new Mock<ITourLogService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new TourLogController(_mockTourLogService.Object, _mockMapper.Object);
    }

    [Test]
    public async Task CreateTourLogAsync_HappyPath_ReturnsCreatedTourLog()
    {
        // Arrange
        var tourLogDto = TestData.CreateSampleTourLogDto();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogDto)).Returns(tourLogDomain);
        _mockTourLogService
            .Setup(s => s.CreateTourLogAsync(tourLogDomain, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tourLogDomain);
        _mockMapper.Setup(m => m.Map<TourLog>(tourLogDomain)).Returns(tourLogDto);

        // Act
        var result = await _controller.CreateTourLog(tourLogDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(tourLogDto));
    }

    [Test]
    public Task CreateTourLogAsync_UnhappyPath_ValidationFails()
    {
        // Arrange
        var tourLogDto = TestData.CreateSampleTourLogDto();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogDto)).Returns(tourLogDomain);
        _mockTourLogService
            .Setup(s => s.CreateTourLogAsync(tourLogDomain, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Invalid tour log data"));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _controller.CreateTourLog(tourLogDto));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task CreateTourLogAsync_UnhappyPath_DuplicateTourLog()
    {
        // Arrange
        var tourLogDto = TestData.CreateSampleTourLogDto();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogDto)).Returns(tourLogDomain);
        _mockTourLogService
            .Setup(s => s.CreateTourLogAsync(tourLogDomain, It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "TourLog with the same date already exists for this tour"
                )
            );

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.CreateTourLog(tourLogDto));
        return Task.CompletedTask;
    }

    [Test]
    public void GetTourLogById_HappyPath_ReturnsTourLog()
    {
        // Arrange
        var tourLogId = Guid.NewGuid();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        var tourLogDto = TestData.CreateSampleTourLogDto();
        _mockTourLogService.Setup(s => s.GetTourLogById(tourLogId)).Returns(tourLogDomain);
        _mockMapper.Setup(m => m.Map<TourLog>(tourLogDomain)).Returns(tourLogDto);

        // Act
        var result = _controller.GetTourLogById(tourLogId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(tourLogDto));
    }

    [Test]
    public void GetTourLogById_UnhappyPath_TourLogNotFound()
    {
        // Arrange
        var tourLogId = TestData.NonexistentGuid;
        _mockTourLogService.Setup(s => s.GetTourLogById(tourLogId)).Returns((TourLogDomain)null!);

        // Act
        var result = _controller.GetTourLogById(tourLogId);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public void GetTourLogsByTourId_HappyPath_ReturnsTourLogs()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tourLogsDomain = TestData.CreateSampleTourLogDomainList();
        var tourLogsDto = TestData.CreateSampleTourLogDtoList();
        _mockTourLogService
            .Setup(s => s.GetTourLogsByTourId(tourId))
            .Returns(tourLogsDomain);
        _mockMapper.Setup(m => m.Map<IEnumerable<TourLog>>(tourLogsDomain)).Returns(tourLogsDto);

        // Act
        var result =  _controller.GetTourLogsByTourId(tourId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(tourLogsDto));
    }

    [Test]
    public void GetTourLogsByTourId_UnhappyPath_TourNotFound()
    {
        // Arrange
        var tourId = TestData.NonexistentGuid;
        _mockTourLogService
            .Setup(s => s.GetTourLogsByTourId(tourId))
            .Throws(new KeyNotFoundException("Tour not found"));

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _controller.GetTourLogsByTourId(tourId));
    }
    
    [Test]
    public void GetTourLogsByTourId_UnhappyPath_DatabaseError()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        _mockTourLogService
            .Setup(s => s.GetTourLogsByTourId(tourId))
            .Throws(new Exception("Database connection error"));

        // Act & Assert
        Assert.Throws<Exception>(() => _controller.GetTourLogsByTourId(tourId));
    }

    [Test]
    public async Task UpdateTourLogAsync_HappyPath_ReturnsUpdatedTourLog()
    {
        // Arrange
        var tourLogId = Guid.NewGuid();
        var tourLogDto = TestData.CreateSampleTourLogDto();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogDto)).Returns(tourLogDomain);
        _mockTourLogService
            .Setup(s => s.UpdateTourLogAsync(tourLogDomain, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tourLogDomain);
        _mockMapper.Setup(m => m.Map<TourLog>(tourLogDomain)).Returns(tourLogDto);

        // Act
        var result = await _controller.UpdateTourLog(tourLogId, tourLogDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(tourLogDto));
    }

    [Test]
    public Task UpdateTourLogAsync_UnhappyPath_ConcurrencyConflict()
    {
        // Arrange
        var tourLogId = TestData.NonexistentGuid;
        var tourLogDto = TestData.CreateSampleTourLogDto();
        var tourLogDomain = TestData.CreateSampleTourLogDomain();
        _mockMapper.Setup(m => m.Map<TourLogDomain>(tourLogDto)).Returns(tourLogDomain);
        _mockTourLogService
            .Setup(s => s.UpdateTourLogAsync(tourLogDomain, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException("Concurrency conflict occurred"));

        // Act & Assert
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => _controller.UpdateTourLog(tourLogId, tourLogDto)
        );
        return Task.CompletedTask;
    }

    [Test]
    public async Task DeleteTourLogAsync_HappyPath_ReturnsNoContent()
    {
        // Arrange
        var tourLogId = Guid.NewGuid();
        _mockTourLogService
            .Setup(s => s.DeleteTourLogAsync(tourLogId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTourLog(tourLogId);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public Task DeleteTourLogAsync_UnhappyPath_TourLogNotFound()
    {
        // Arrange
        var tourLogId = TestData.NonexistentGuid;
        _mockTourLogService
            .Setup(s => s.DeleteTourLogAsync(tourLogId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("TourLog not found"));

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.DeleteTourLog(tourLogId));
        return Task.CompletedTask;
    }
}