using System.Net;
using API.AOP;
using BL.DomainModel;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using UI.Model;

namespace API.Controllers;

[ApiController]
[Route("api/tour")]
public class TourController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITourService _tourService;

    public TourController(ITourService tourService, IMapper mapper)
    {
        _tourService = tourService;
        _mapper = mapper;
    }

    [ApiMethodDecorator]
    [HttpPost]
    [ProducesResponseType(typeof(Tour), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Tour>> CreateTour([FromBody] Tour tourDto)
    {
        var tourDomain = _mapper.Map<TourDomain>(tourDto);
        var createdTour = await _tourService.CreateTourAsync(tourDomain);
        return Ok(_mapper.Map<Tour>(createdTour));
    }

    [ApiMethodDecorator]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Tour>), (int)HttpStatusCode.OK)]
    public ActionResult<IEnumerable<Tour>> GetAllTours()
    {
        var tours = _tourService.GetAllTours();
        return Ok(_mapper.Map<IEnumerable<Tour>>(tours));
    }

    [ApiMethodDecorator]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Tour), (int)HttpStatusCode.OK)]
    public ActionResult<Tour> GetTourById(Guid id)
    {
        var tour = _tourService.GetTourById(id);
        return Ok(_mapper.Map<Tour>(tour));
    }

    [ApiMethodDecorator]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Tour), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Tour>> UpdateTour(Guid id, [FromBody] Tour tourDto)
    {
        if (id != tourDto.Id)
        {
            return BadRequest("ID mismatch");
        }
        var tourDomain = _mapper.Map<TourDomain>(tourDto);
        var updatedTour = await _tourService.UpdateTourAsync(tourDomain);
        return Ok(_mapper.Map<Tour>(updatedTour));
    }

    [ApiMethodDecorator]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteTour(Guid id)
    {
        await _tourService.DeleteTourAsync(id);
        return NoContent();
    }

    [ApiMethodDecorator]
    [HttpGet("search/{searchText}")]
    [ProducesResponseType(typeof(IEnumerable<Tour>), (int)HttpStatusCode.OK)]
    public ActionResult SearchTours(string searchText)
    {
        var tours = _tourService.SearchTours(searchText);
        var tourDtos = _mapper.Map<IEnumerable<Tour>>(tours);
        return Ok(tourDtos);
    }
}