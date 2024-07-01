using System.Text.Json;
using API.Controllers;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UI.Model;

namespace Test;

[TestFixture]
public class FileControllerTests
{
    [SetUp]
    public void Setup()
    {
        _mockFileService = new Mock<IFileService>();
        _mockTourService = new Mock<ITourService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new FileController(
        _mockFileService.Object,
        _mockTourService.Object,
        _mockMapper.Object
        );
    }

    private Mock<IFileService> _mockFileService;
    private Mock<ITourService> _mockTourService;
    private Mock<IMapper> _mockMapper;
    private FileController _controller;

    [Test]
    public async Task GetSummaryReport_HappyPath_ReturnsPdfFile()
    {
        // Arrange
        var tours = TestData.CreateSampleTourDomainList();
        byte[] pdfBytes =
        {
            1, 2, 3
        };
        _mockTourService.Setup(s => s.GetAllToursAsync()).ReturnsAsync(tours);
        _mockFileService.Setup(s => s.GenerateSummaryReportAsync(tours)).ReturnsAsync(pdfBytes);

        // Act
        var result = await _controller.GetSummaryReport();

        // Assert
        Assert.That(result, Is.TypeOf<FileContentResult>());
        var fileResult = (FileContentResult)result;
        Assert.Multiple(() => {
            Assert.That(fileResult.FileContents, Is.EqualTo(pdfBytes));
            Assert.That(fileResult.ContentType, Is.EqualTo("application/pdf"));
            Assert.That(fileResult.FileDownloadName, Is.EqualTo("SummaryReport.pdf"));
        });
    }

    [Test]
    public Task GetSummaryReport_UnhappyPath_ThrowsException()
    {
        // Arrange
        _mockTourService
            .Setup(s => s.GetAllToursAsync())
            .ThrowsAsync(new Exception("Database connection error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _controller.GetSummaryReport());
        return Task.CompletedTask;
    }

    [Test]
    public async Task GetTourReport_HappyPath_ReturnsPdfFile()
    {
        // Arrange
        var tourId = TestData.TestGuid;
        byte[] pdfBytes =
        {
            4, 5, 6
        };
        _mockFileService.Setup(s => s.GenerateTourReportAsync(tourId)).ReturnsAsync(pdfBytes);

        // Act
        var result = await _controller.GetTourReport(tourId);

        // Assert
        Assert.That(result, Is.TypeOf<FileContentResult>());
        var fileResult = (FileContentResult)result;
        Assert.Multiple(() => {
            Assert.That(fileResult.FileContents, Is.EqualTo(pdfBytes));
            Assert.That(fileResult.ContentType, Is.EqualTo("application/pdf"));
            Assert.That(fileResult.FileDownloadName, Is.EqualTo($"TourReport_{tourId}.pdf"));
        });
    }

    [Test]
    public Task GetTourReport_UnhappyPath_ReportGenerationFails()
    {
        // Arrange
        var tourId = TestData.NonexistentGuid;
        _mockFileService
            .Setup(s => s.GenerateTourReportAsync(tourId))
            .ThrowsAsync(new InvalidOperationException("Report generation failed"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetTourReport(tourId));
        return Task.CompletedTask;
    }

    [Test]
    public async Task ExportTourToJson_HappyPath_ReturnsJsonResult()
    {
        // Arrange
        var tourId = Guid.NewGuid();
        var tourDomain = TestData.CreateSampleTourDomain();
        var tourDto = TestData.CreateSampleTour();
        _mockFileService.Setup(s => s.ExportTourToJsonAsync(tourId)).ReturnsAsync(tourDomain);
        _mockMapper.Setup(m => m.Map<Tour>(tourDomain)).Returns(tourDto);

        // Act
        var result = await _controller.ExportTourToJson(tourId);

        // Assert
        Assert.That(result, Is.TypeOf<JsonResult>());
        var jsonResult = (JsonResult)result;
        Assert.Multiple(() => {
            Assert.That(jsonResult.Value, Is.EqualTo(tourDto));
            Assert.That(jsonResult.ContentType, Is.EqualTo("application/json"));
            Assert.That(jsonResult.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public Task ExportTourToJson_UnhappyPath_TourNotFound()
    {
        // Arrange
        var tourId = TestData.NonexistentGuid;
        _mockFileService
            .Setup(s => s.ExportTourToJsonAsync(tourId))
            .ThrowsAsync(new KeyNotFoundException("Tour not found"));

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.ExportTourToJson(tourId));
        return Task.CompletedTask;
    }

    [Test]
    public async Task ImportTourFromJson_HappyPath_ReturnsOkResult()
    {
        // Arrange
        var json = TestData.CreateSampleTourJson();

        // Act
        var result = await _controller.ImportTourFromJson(json);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo("Tour imported successfully"));
    }

    [Test]
    public Task ImportTourFromJson_UnhappyPath_InvalidJsonFormat()
    {
        // Arrange
        const string invalidJson = "{invalid_json}";
        _mockFileService
            .Setup(s => s.ImportTourFromJsonAsync(invalidJson))
            .ThrowsAsync(new JsonException("Invalid JSON format"));

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(() => _controller.ImportTourFromJson(invalidJson));
        return Task.CompletedTask;
    }

    [Test]
    public Task ImportTourFromJson_UnhappyPath_DuplicateTourData()
    {
        // Arrange
        var json = TestData.CreateSampleTourJson();
        _mockFileService
            .Setup(s => s.ImportTourFromJsonAsync(json))
            .ThrowsAsync(new InvalidOperationException("Tour with the same ID already exists"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.ImportTourFromJson(json));
        return Task.CompletedTask;
    }

    [Test]
    public Task GetSummaryReport_UnhappyPath_PdfGenerationFails()
    {
        // Arrange
        var tours = TestData.CreateSampleTourDomainList();
        _mockTourService.Setup(s => s.GetAllToursAsync()).ReturnsAsync(tours);
        _mockFileService
            .Setup(s => s.GenerateSummaryReportAsync(tours))
            .ThrowsAsync(new InvalidOperationException("PDF generation failed"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetSummaryReport());
        return Task.CompletedTask;
    }
}
