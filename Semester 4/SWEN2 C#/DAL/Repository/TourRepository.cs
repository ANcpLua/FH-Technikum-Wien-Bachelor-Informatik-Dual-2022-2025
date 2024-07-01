using DAL.Infrastructure;
using DAL.Interface;
using DAL.PersistenceModel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class TourRepository : ITourRepository
{
    private readonly TourPlannerContext _dbContext;

    public TourRepository(TourPlannerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TourPersistence> CreateTourAsync(TourPersistence tour)
    {
        _dbContext.Set<TourPersistence>().Add(tour);
        await _dbContext.SaveChangesAsync();
        return tour;
    }

    public async Task<IEnumerable<TourPersistence>> GetAllToursAsync() => await _dbContext
        .Set<TourPersistence>()
        .Include(t => t.TourLogPersistence)
        .ToListAsync();

    public TourPersistence? GetTourById(Guid id) => _dbContext
        .Set<TourPersistence>()
        .Include(t => t.TourLogPersistence)
        .FirstOrDefault(t => t.Id == id);

    public async Task<TourPersistence> UpdateTourAsync(TourPersistence tour)
    {
        _dbContext.Set<TourPersistence>().Update(tour);
        await _dbContext.SaveChangesAsync();
        return tour;
    }

    public async Task DeleteTourAsync(Guid id)
    {
        var tour = await _dbContext.Set<TourPersistence>().FindAsync(id);
        if (tour != null)
        {
            _dbContext.Set<TourPersistence>().Remove(tour);
            await _dbContext.SaveChangesAsync();
        }
    }

    public IQueryable<TourPersistence> SearchToursAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return _dbContext.ToursPersistence;
        }

        return _dbContext
            .ToursPersistence.Include(t => t.TourLogPersistence)
            .Where(t =>
                t.Name.Contains(searchText) ||
                t.Description.Contains(searchText) ||
                t.From.Contains(searchText) ||
                t.To.Contains(searchText) ||
                t.TourLogPersistence.Any(tl => tl.Comment.Contains(searchText))
            );
    }
}
