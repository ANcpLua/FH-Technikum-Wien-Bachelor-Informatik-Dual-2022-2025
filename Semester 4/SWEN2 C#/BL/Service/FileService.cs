using System.Text.Json;
using BL.DomainModel;
using BL.Interface;

namespace BL.Service;

public class FileService : IFileService
{
    private readonly IPdfReportService _pdfReportService;
    private readonly ITourService _tourService;

    public FileService(ITourService tourService, IPdfReportService pdfReportService)
    {
        _tourService = tourService;
        _pdfReportService = pdfReportService;
    }

    public byte[] GenerateTourReport(Guid tourId)
    {
        var tour = _tourService.GetTourById(tourId);
        return _pdfReportService.GenerateTourReport(tour);
    }

    public byte[] GenerateSummaryReport(IEnumerable<TourDomain> tours) => _pdfReportService.GenerateSummaryReport(tours);

    public TourDomain ExportTourToJson(Guid tourId)
    {
        var tour = _tourService.GetTourById(tourId);
        return tour;
    }

    public async Task ImportTourFromJsonAsync(string json)
    {
        var tour = JsonSerializer.Deserialize<TourDomain>(json);
        if (tour != null)
        {
            await _tourService.CreateTourAsync(tour);
        }
    }
}