using System.Text.Json;
using BL.DomainModel;
using BL.Interface;
using BL.Service;
using Moq;

namespace Test;

[TestFixture]
public class FileServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockTourService = new Mock<ITourService>();
        _mockPdfReportService = new Mock<IPdfReportService>();
        _fileService = new FileService(_mockTourService.Object, _mockPdfReportService.Object);
    }

    private Mock<ITourService> _mockTourService;
    private Mock<IPdfReportService> _mockPdfReportService;
    private FileService _fileService;

    [Test]
    public async Task GenerateTourReportAsync_ValidTourId_ReturnsPdfBytes()
    {
        // Arrange
        var tourId = TestData.TestGuid;
        var tour = TestData.CreateSampleTourDomain();
        byte[] expectedPdfBytes =
        {
            1, 2, 3, 4, 5
        };

        _mockTourService.Setup(s => s.GetTourById(tourId)).Returns(tour);
        _mockPdfReportService.Setup(s => s.GenerateTourReport(tour)).Returns(expectedPdfBytes);

        // Act
        var result = await _fileService.GenerateTourReportAsync(tourId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedPdfBytes));
        _mockTourService.Verify(s => s.GetTourById(tourId), Times.Once);
        _mockPdfReportService.Verify(s => s.GenerateTourReport(tour), Times.Once);
    }

    [Test]
    public async Task GenerateSummaryReportAsync_ValidTours_ReturnsPdfBytes()
    {
        // Arrange
        var tours = TestData.CreateSampleTourDomainList();
        byte[] expectedPdfBytes =
        {
            1, 2, 3, 4, 5
        };

        _mockPdfReportService.Setup(s => s.GenerateSummaryReport(tours)).Returns(expectedPdfBytes);

        // Act
        var result = await _fileService.GenerateSummaryReportAsync(tours);

        // Assert
        Assert.That(result, Is.EqualTo(expectedPdfBytes));
        _mockPdfReportService.Verify(s => s.GenerateSummaryReport(tours), Times.Once);
    }

    [Test]
    public async Task ExportTourToJsonAsync_ValidTourId_ReturnsTourDomain()
    {
        // Arrange
        var tourId = TestData.TestGuid;
        var expectedTour = TestData.CreateSampleTourDomain();

        _mockTourService.Setup(s => s.GetTourById(tourId)).Returns(expectedTour);

        // Act
        var result = await _fileService.ExportTourToJsonAsync(tourId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTour));
        _mockTourService.Verify(s => s.GetTourById(tourId), Times.Once);
    }

    [Test]
    public async Task ImportTourFromJsonAsync_ValidJson_CreatesTour()
    {
        // Arrange
        var expectedTour = TestData.CreateSampleTourDomain();
        var json = TestData.CreateSampleTourDomainJson();

        _mockTourService
            .Setup(s => s.CreateTourAsync(It.IsAny<TourDomain>()))
            .ReturnsAsync(expectedTour);

        // Act
        await _fileService.ImportTourFromJsonAsync(json);

        // Assert
        _mockTourService.Verify(
        s => s.CreateTourAsync(It.Is<TourDomain>(t => t.Id == expectedTour.Id)),
        Times.Once
        );
    }

    [Test]
    public async Task GenerateSummaryReportAsync_LargeTourList_HandlesLargeDataSet()
    {
        // Arrange
        var largeTourList = Enumerable
            .Range(0, 1000)
            .Select(_ => TestData.CreateSampleTourDomain())
            .ToList();
        var expectedPdfBytes = new byte[1024 * 1024];// 1MB of data

        _mockPdfReportService
            .Setup(s => s.GenerateSummaryReport(largeTourList))
            .Returns(expectedPdfBytes);

        // Act
        var result = await _fileService.GenerateSummaryReportAsync(largeTourList);

        // Assert
        Assert.That(result, Is.EqualTo(expectedPdfBytes));
        _mockPdfReportService.Verify(s => s.GenerateSummaryReport(largeTourList), Times.Once);
    }

    [Test]
    public async Task ExportTourToJsonAsync_TourWithLargeLogs_HandlesLargeDataSet()
    {
        // Arrange
        var tourId = TestData.TestGuid;
        var tourWithLargeLogs = TestData.CreateSampleTourDomain();
        tourWithLargeLogs.Logs = Enumerable
            .Range(0, 10000)
            .Select(_ => TestData.CreateSampleTourLogDomain())
            .ToList();

        _mockTourService.Setup(s => s.GetTourById(tourId)).Returns(tourWithLargeLogs);

        // Act
        var result = await _fileService.ExportTourToJsonAsync(tourId);

        // Assert
        Assert.That(result, Is.EqualTo(tourWithLargeLogs));
        Assert.That(result.Logs.Count, Is.EqualTo(10000));
        _mockTourService.Verify(s => s.GetTourById(tourId), Times.Once);
    }

    [Test]
    public async Task ExportTourToJsonAsync_InvalidTourId_ReturnsNull()
    {
        // Arrange
        var invalidTourId = TestData.NonexistentGuid;
        _mockTourService.Setup(s => s.GetTourById(invalidTourId)).Returns((TourDomain)null!);

        // Act
        var result = await _fileService.ExportTourToJsonAsync(invalidTourId);

        // Assert
        Assert.That(result, Is.Null);
        _mockTourService.Verify(s => s.GetTourById(invalidTourId), Times.Once);
    }

    [Test]
    public async Task GenerateTourReportAsync_InvalidTourId_ReturnsEmptyByteArray()
    {
        // Arrange
        var invalidTourId = TestData.NonexistentGuid;
        _mockTourService.Setup(s => s.GetTourById(invalidTourId)).Returns((TourDomain)null!);
        _mockPdfReportService.Setup(s => s.GenerateTourReport(null!)).Returns(new byte[0]);

        // Act
        var result = await _fileService.GenerateTourReportAsync(invalidTourId);

        // Assert
        Assert.That(result, Is.Empty);
        _mockTourService.Verify(s => s.GetTourById(invalidTourId), Times.Once);
        _mockPdfReportService.Verify(s => s.GenerateTourReport(null!), Times.Once);
    }

    [Test]
    public void ImportTourFromJsonAsync_InvalidJson_DoesNotCreateTour()
    {
        // Arrange
        const string invalidJson = "{invalid json}";
        _mockTourService
            .Setup(s => s.CreateTourAsync(It.IsAny<TourDomain>()))
            .ReturnsAsync((TourDomain)null!);

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(() => _fileService.ImportTourFromJsonAsync(invalidJson));
        _mockTourService.Verify(s => s.CreateTourAsync(It.IsAny<TourDomain>()), Times.Never);
    }
}
