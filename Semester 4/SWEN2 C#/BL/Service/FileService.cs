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

    public Task<byte[]> GenerateTourReportAsync(Guid tourId)
    {
        var tour = _tourService.GetTourById(tourId);
        return Task.FromResult(_pdfReportService.GenerateTourReport(tour));
    }

    public Task<byte[]> GenerateSummaryReportAsync(IEnumerable<TourDomain> tours) => Task.FromResult(_pdfReportService.GenerateSummaryReport(tours));

    public Task<TourDomain> ExportTourToJsonAsync(Guid tourId)
    {
        var tour = _tourService.GetTourById(tourId);
        return Task.FromResult(tour);
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
