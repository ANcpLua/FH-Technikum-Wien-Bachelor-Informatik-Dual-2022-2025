using DAL.Infrastructure;
using DAL.Interface;
using DAL.PersistenceModel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class TourLogRepository : ITourLogRepository
{
    private readonly TourPlannerContext _dbContext;

    public TourLogRepository(TourPlannerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TourLogPersistence> CreateTourLogAsync(
        TourLogPersistence newTourLogPersistence,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.TourLogsPersistence.Add(newTourLogPersistence);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newTourLogPersistence;
    }

    public async Task<IEnumerable<TourLogPersistence>> GetTourLogsByTourIdAsync(
        Guid tourId,
        CancellationToken cancellationToken = default
    ) => await _dbContext
        .TourLogsPersistence.Where(t => t.TourPersistenceId == tourId)
        .ToListAsync(cancellationToken);

    public TourLogPersistence? GetTourLogById(Guid id) => _dbContext.TourLogsPersistence.FirstOrDefault(t => t.Id == id);

    public async Task<TourLogPersistence> UpdateTourLogAsync(
        TourLogPersistence updatedTourLogPersistence,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.TourLogsPersistence.Update(updatedTourLogPersistence);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return updatedTourLogPersistence;
    }

    public async Task DeleteTourLogAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tourLogPersistence = await _dbContext.TourLogsPersistence.FirstOrDefaultAsync(
        t => t.Id == id,
        cancellationToken
        );
        _dbContext.TourLogsPersistence.Remove(tourLogPersistence!);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
