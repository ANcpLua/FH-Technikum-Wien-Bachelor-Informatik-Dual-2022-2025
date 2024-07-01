using DAL.PersistenceModel;

namespace DAL.Interface;

public interface ITourRepository
{
    Task<TourPersistence> CreateTourAsync(TourPersistence tour);
    Task<IEnumerable<TourPersistence>> GetAllToursAsync();
    TourPersistence? GetTourById(Guid id);
    Task<TourPersistence> UpdateTourAsync(TourPersistence tour);
    Task DeleteTourAsync(Guid id);
    IQueryable<TourPersistence> SearchToursAsync(string searchText);
}
