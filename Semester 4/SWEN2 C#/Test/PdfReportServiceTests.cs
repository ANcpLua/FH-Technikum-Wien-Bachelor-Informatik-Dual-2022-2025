using System.Text;
using BL.Service;

namespace Test;

[TestFixture]
public class PdfReportServiceTests
{
    [SetUp]
    public void Setup() => _pdfReportService = new PdfReportService();

    private PdfReportService _pdfReportService;

    [Test]
    public void GenerateTourReport_ValidTour_ReturnsPdfBytes()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Empty);
            Assert.That(Encoding.UTF8.GetString(result.Take(4).ToArray()), Is.EqualTo("%PDF"));
        });
    }

    [Test]
    public void GenerateSummaryReport_ValidTours_ReturnsPdfBytes()
    {
        // Arrange
        var tours = TestData.CreateSampleTourDomainList();

        // Act
        var result = _pdfReportService.GenerateSummaryReport(tours);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Empty);
            Assert.That(Encoding.UTF8.GetString(result.Take(4).ToArray()), Is.EqualTo("%PDF"));
        });
    }

    [Test]
    public void GenerateTourReport_TourWithNoLogs_GeneratesReportWithoutLogs()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.Logs = [];

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateTourReport_TourWithInvalidImagePath_GeneratesReportWithoutImage()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.ImagePath = "invalid/path/to/image.png";

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateSummaryReport_EmptyTourList_GeneratesEmptyReport()
    {
        // Arrange
        var emptyTourList = TestData.CreateSampleTourDomainList();
        emptyTourList.Clear();

        // Act
        var result = _pdfReportService.GenerateSummaryReport(emptyTourList);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateTourReport_TourWithLongDescription_HandlesLongText()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.Description = new string('A', 10000);

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateSummaryReport_LargeTourList_HandlesLargeDataSet()
    {
        // Arrange
        var largeTourList = Enumerable
            .Range(0, 1000)
            .Select(_ => TestData.CreateSampleTourDomain())
            .ToList();

        // Act
        var result = _pdfReportService.GenerateSummaryReport(largeTourList);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateTourReport_TourWithSpecialCharacters_HandlesSpecialCharacters()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.Name = "Tour with special characters: áéíóú ñ ¿¡ €";

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateTourReport_TourWithExtremeValues_HandlesExtremeValues()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.Distance = double.MaxValue;
        tour.EstimatedTime = double.MinValue;

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void GenerateTourReport_TourWithMaximumLogs_HandlesLargeNumberOfLogs()
    {
        // Arrange
        var tour = TestData.CreateSampleTourDomain();
        tour.Logs = Enumerable
            .Range(0, 1000)
            .Select(_ => TestData.CreateSampleTourLogDomain())
            .ToList();

        // Act
        var result = _pdfReportService.GenerateTourReport(tour);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }
}
