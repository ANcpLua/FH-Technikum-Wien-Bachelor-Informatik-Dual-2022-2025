using System.Net;
using API.AOP;
using BL.DomainModel;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using UI.Model;

namespace API.Controllers;

[ApiController]
[Route("api/tourlog")]
public class TourLogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITourLogService _tourLogService;

    public TourLogController(ITourLogService tourLogService, IMapper mapper)
    {
        _tourLogService = tourLogService;
        _mapper = mapper;
    }

    [ApiMethodDecorator]
    [HttpPost]
    [ProducesResponseType(typeof(TourLog), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateTourLog(
        [FromBody] TourLog tourLogDto,
        CancellationToken cancellationToken = default
    )
    {
        var tourLog = _mapper.Map<TourLogDomain>(tourLogDto);
        var createdTourLog = await _tourLogService.CreateTourLogAsync(tourLog, cancellationToken);
        var createdTourLogDto = _mapper.Map<TourLog>(createdTourLog);
        return Ok(createdTourLogDto);
    }

    [ApiMethodDecorator]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TourLog), (int)HttpStatusCode.OK)]
    public ActionResult GetTourLogById(Guid id)
    {
        var tourLog = _tourLogService.GetTourLogById(id);
        if (tourLog == null)
        {
            return NotFound();
        }
        var tourLogDto = _mapper.Map<TourLog>(tourLog);
        return Ok(tourLogDto);
    }

    [ApiMethodDecorator]
    [HttpGet("bytour/{tourId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<TourLog>), (int)HttpStatusCode.OK)]
    public ActionResult GetTourLogsByTourId(
        Guid tourId
    )
    {
        var tourLogs =  _tourLogService.GetTourLogsByTourId(tourId);
        var tourLogDtos = _mapper.Map<IEnumerable<TourLog>>(tourLogs);
        return Ok(tourLogDtos);
    }

    [ApiMethodDecorator]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TourLog), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateTourLog(
        Guid id,
        [FromBody] TourLog tourLogDto,
        CancellationToken cancellationToken = default
    )
    {
        var tourLog = _mapper.Map<TourLogDomain>(tourLogDto);
        var updatedTourLog = await _tourLogService.UpdateTourLogAsync(tourLog, cancellationToken);
        var updatedTourLogDto = _mapper.Map<TourLog>(updatedTourLog);
        return Ok(updatedTourLogDto);
    }

    [ApiMethodDecorator]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteTourLog(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        await _tourLogService.DeleteTourLogAsync(id, cancellationToken);
        return NoContent();
    }
}