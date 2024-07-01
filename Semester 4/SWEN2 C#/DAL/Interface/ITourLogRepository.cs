using DAL.PersistenceModel;

namespace DAL.Interface;

public interface ITourLogRepository
{
    Task<TourLogPersistence> CreateTourLogAsync(
        TourLogPersistence newTourLogPersistence,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<TourLogPersistence>> GetTourLogsByTourIdAsync(
        Guid tourId,
        CancellationToken cancellationToken = default
    );
    TourLogPersistence? GetTourLogById(Guid id);
    Task<TourLogPersistence> UpdateTourLogAsync(
        TourLogPersistence updatedTourLogPersistence,
        CancellationToken cancellationToken = default
    );
    Task DeleteTourLogAsync(Guid id, CancellationToken cancellationToken = default);
}
