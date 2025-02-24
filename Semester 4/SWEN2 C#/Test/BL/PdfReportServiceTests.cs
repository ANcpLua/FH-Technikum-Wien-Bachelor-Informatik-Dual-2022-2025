using System.Text;
using BL.DomainModel;
using BL.Service;

namespace Test.BL;

[TestFixture]
public class PdfReportServiceTests
{
    private PdfReportService _pdfReportService = null!;
    private const string PdfHeader = "%PDF";

    [SetUp]
    public void Setup() => _pdfReportService = new PdfReportService();

    [Test]
    public void GenerateTourReport_ValidTour_ReturnsPdfBytes()
    {
        var tour = TestData.CreateSampleTourDomain();
        var result = _pdfReportService.GenerateTourReport(tour);
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateSummaryReport_ValidTours_ReturnsPdfBytes()
    {
        var tours = TestData.CreateSampleTourDomainList();
        var result = _pdfReportService.GenerateSummaryReport(tours);
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateTourReport_InvalidImagePath_HandlesInvalidPath()
    {
        var tour = TestData.CreateSampleTourDomain();
        tour.ImagePath = "invalid/path/to/image.png";

        var result = _pdfReportService.GenerateTourReport(tour);
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateSummaryReport_EmptyTourList_GeneratesEmptyReport()
    {
        var result = _pdfReportService.GenerateSummaryReport(Array.Empty<TourDomain>());
        
        AssertValidPdf(result);
    }
    
    [Test]
    public void GenerateTourReport_EmptyTour_GeneratesEmptyReport()
    {
        var result = _pdfReportService.GenerateTourReport(new TourDomain());
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateTourReport_ExtremeValues_HandlesExtremeValues()
    {
        var tour = TestData.CreateSampleTourDomain();
        tour.Distance = double.MaxValue;
        tour.EstimatedTime = double.MinValue;
        
        foreach (var log in tour.Logs)
        {
            log.TotalDistance = double.MaxValue;
            log.TotalTime = double.MinValue;
            log.Rating = int.MaxValue;
        }

        var result = _pdfReportService.GenerateTourReport(tour);
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateTourReport_SpecialCharacters_HandlesSpecialCharacters()
    {
        var tour = TestData.CreateSampleTourDomain();
        const string specialChars = "Special characters: áéíóú ñ ¿¡ € &<>\"'";
        tour.Name = specialChars;
        tour.Description = specialChars;
        tour.From = specialChars;
        tour.To = specialChars;
        
        foreach (var log in tour.Logs)
        {
            log.Comment = specialChars;
        }

        var result = _pdfReportService.GenerateTourReport(tour);
        
        AssertValidPdf(result);
    }

    [Test]
    public void GenerateReport_LargeDataSets_HandlesLargeData()
    {
        var tours = Enumerable.Range(0, 100)
            .Select(_ => TestData.CreateSampleTourDomain())
            .ToList();

        foreach (var tour in tours)
        {
            tour.Description = new string('A', 1000);
            tour.Logs = Enumerable.Range(0, 50)
                .Select(_ => TestData.CreateSampleTourLogDomain())
                .ToList();
        }

        var result = _pdfReportService.GenerateSummaryReport(tours);
        
        AssertValidPdf(result);
    }

    private static void AssertValidPdf(byte[] pdfBytes)
    {
        Assert.Multiple(() =>
        {
            Assert.That(pdfBytes, Is.Not.Null);
            Assert.That(pdfBytes, Is.Not.Empty);
            Assert.That(Encoding.UTF8.GetString(pdfBytes.Take(4).ToArray()), Is.EqualTo(PdfHeader));
        });
    }
}