using DAL.PersistenceModel;

namespace DAL.Interface;

public interface ITourRepository
{
    Task<TourPersistence> CreateTourAsync(TourPersistence tour);
    IEnumerable<TourPersistence> GetAllTours();
    TourPersistence? GetTourById(Guid id);
    Task<TourPersistence> UpdateTourAsync(TourPersistence tour);
    Task DeleteTourAsync(Guid id);
    IQueryable<TourPersistence> SearchToursAsync(string searchText);
}