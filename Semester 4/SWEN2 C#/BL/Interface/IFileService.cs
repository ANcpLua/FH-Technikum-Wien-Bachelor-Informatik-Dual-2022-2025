using BL.DomainModel;

namespace BL.Interface;

public interface IFileService
{
    Task<byte[]> GenerateTourReportAsync(Guid tourId);
    Task<TourDomain> ExportTourToJsonAsync(Guid tourId);
    Task<byte[]> GenerateSummaryReportAsync(IEnumerable<TourDomain> tours);
    Task ImportTourFromJsonAsync(string json);
}
