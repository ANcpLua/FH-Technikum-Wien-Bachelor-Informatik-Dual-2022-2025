using BL.DomainModel;
using BL.Interface;
using DAL.Interface;
using DAL.PersistenceModel;
using MapsterMapper;

namespace BL.Service;

public class TourLogService : ITourLogService
{
    private readonly IMapper _mapper;
    private readonly ITourLogRepository _tourLogRepository;

    public TourLogService(ITourLogRepository tourLogRepository, IMapper mapper)
    {
        _tourLogRepository = tourLogRepository;
        _mapper = mapper;
    }

    public async Task<TourLogDomain> CreateTourLogAsync(
        TourLogDomain tourLog,
        CancellationToken cancellationToken = default
    )
    {
        var tourLogPersistence = _mapper.Map<TourLogPersistence>(tourLog);
        var createdTourLogPersistence = await _tourLogRepository.CreateTourLogAsync(
        tourLogPersistence,
        cancellationToken
        );
        return _mapper.Map<TourLogDomain>(createdTourLogPersistence);
    }

    public TourLogDomain? GetTourLogById(Guid id)
    {
        var tourLogPersistence = _tourLogRepository.GetTourLogById(id);
        return tourLogPersistence == null ? null : _mapper.Map<TourLogDomain>(tourLogPersistence);
    }

    public IEnumerable<TourLogDomain> GetTourLogsByTourId(
        Guid tourId
    )
    {
        var tourLogPersistence =  _tourLogRepository.GetTourLogsByTourId(
        tourId
        );
        return _mapper.Map<IEnumerable<TourLogDomain>>(tourLogPersistence);
    }

    public async Task<TourLogDomain> UpdateTourLogAsync(
        TourLogDomain tourLog,
        CancellationToken cancellationToken = default
    )
    {
        var tourLogPersistence = _mapper.Map<TourLogPersistence>(tourLog);
        var updatedTourLogPersistence = await _tourLogRepository.UpdateTourLogAsync(
        tourLogPersistence,
        cancellationToken
        );
        return _mapper.Map<TourLogDomain>(updatedTourLogPersistence);
    }

    public Task DeleteTourLogAsync(Guid id, CancellationToken cancellationToken = default) => _tourLogRepository.DeleteTourLogAsync(id, cancellationToken);
}