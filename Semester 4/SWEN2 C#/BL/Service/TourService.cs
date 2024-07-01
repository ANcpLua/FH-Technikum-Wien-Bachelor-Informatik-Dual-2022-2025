using BL.DomainModel;
using BL.Interface;
using DAL.Interface;
using DAL.PersistenceModel;
using MapsterMapper;

namespace BL.Service;

public class TourService : ITourService
{
    private readonly IMapper _mapper;
    private readonly ITourRepository _tourRepository;

    public TourService(ITourRepository tourRepository, IMapper mapper)
    {
        _tourRepository = tourRepository;
        _mapper = mapper;
    }

    public async Task<TourDomain> CreateTourAsync(TourDomain tour)
    {
        var tourPersistence = _mapper.Map<TourPersistence>(tour);
        var createdTour = await _tourRepository.CreateTourAsync(tourPersistence);
        return _mapper.Map<TourDomain>(createdTour);
    }

    public async Task<IEnumerable<TourDomain>> GetAllToursAsync()
    {
        var tours = await _tourRepository.GetAllToursAsync();
        return _mapper.Map<IEnumerable<TourDomain>>(tours);
    }

    public TourDomain GetTourById(Guid id)
    {
        var tourPersistence = _tourRepository.GetTourById(id);
        return (tourPersistence == null ? null : _mapper.Map<TourDomain>(tourPersistence))!;
    }

    public async Task<TourDomain> UpdateTourAsync(TourDomain tour)
    {
        var tourPersistence = _mapper.Map<TourPersistence>(tour);
        var updatedTour = await _tourRepository.UpdateTourAsync(tourPersistence);
        return _mapper.Map<TourDomain>(updatedTour);
    }

    public Task DeleteTourAsync(Guid id) => _tourRepository.DeleteTourAsync(id);

    public IQueryable<TourDomain> SearchTours(string searchText)
    {
        var tourPersistence = _tourRepository.SearchToursAsync(searchText);
        return tourPersistence.Select(t => _mapper.Map<TourDomain>(t));
    }
}
