using BL.DomainModel;

namespace BL.Interface;

public interface ITourLogService
{
    Task<TourLogDomain> CreateTourLogAsync(
        TourLogDomain tourLog,
        CancellationToken cancellationToken = default
    );
    IEnumerable<TourLogDomain> GetTourLogsByTourId(
        Guid tourId
    );
    TourLogDomain? GetTourLogById(Guid id);
    Task<TourLogDomain> UpdateTourLogAsync(
        TourLogDomain tourLog,
        CancellationToken cancellationToken = default
    );
    Task DeleteTourLogAsync(Guid id, CancellationToken cancellationToken = default);
}