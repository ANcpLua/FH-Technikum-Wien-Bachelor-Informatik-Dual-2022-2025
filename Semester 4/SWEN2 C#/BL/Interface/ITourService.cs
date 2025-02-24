using BL.DomainModel;

namespace BL.Interface;

public interface ITourService
{
    Task<TourDomain> CreateTourAsync(TourDomain tour);
    IEnumerable<TourDomain> GetAllTours();
    TourDomain GetTourById(Guid id);
    Task<TourDomain> UpdateTourAsync(TourDomain tour);
    Task DeleteTourAsync(Guid id);
    IQueryable<TourDomain> SearchTours(string searchText);
}