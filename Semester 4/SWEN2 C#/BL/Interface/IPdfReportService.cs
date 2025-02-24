using BL.DomainModel;

namespace BL.Interface;

public interface IPdfReportService
{
    byte[] GenerateTourReport(TourDomain tour);
    byte[] GenerateSummaryReport(IEnumerable<TourDomain> tours);
}