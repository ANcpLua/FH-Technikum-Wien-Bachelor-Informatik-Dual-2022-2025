using System.Net;
using System.Text.Json;
using API.AOP;
using BL.Interface;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using UI.Model;

namespace API.Controllers;

[ApiController]
[Route("api/reports")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly ITourService _tourService;

    public FileController(IFileService fileService, ITourService tourService, IMapper mapper)
    {
        _fileService = fileService;
        _tourService = tourService;
        _mapper = mapper;
    }

    [ApiMethodDecorator]
    [HttpGet("summary")]
    [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
    public FileResult GetSummaryReport()
    {
        var tours = _tourService.GetAllTours();
        var report =  _fileService.GenerateSummaryReport(tours);
        return File(report, "application/pdf", "SummaryReport.pdf");
    }
    
    [ApiMethodDecorator]
    [HttpGet("tour/{tourId:guid}")]
    [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
    public FileResult GetTourReport(Guid tourId)
    {
        var report = _fileService.GenerateTourReport(tourId);
        return File(report, "application/pdf", $"TourReport_{tourId}.pdf");
    }

    [ApiMethodDecorator]
    [HttpGet("export/{tourId:guid}")]
    [ProducesResponseType(typeof(Tour), (int)HttpStatusCode.OK)]
    public ActionResult ExportTourToJson(Guid tourId)
    {
        var tourDomain = _fileService.ExportTourToJson(tourId);
        var tourDto = _mapper.Map<Tour>(tourDomain);
        return new JsonResult(
            tourDto,
            new JsonSerializerOptions
            {
                WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        )
        {
            ContentType = "application/json", StatusCode = (int)HttpStatusCode.OK
        };
    }

    [ApiMethodDecorator]
    [HttpPost("import")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportTourFromJsonAsync([FromBody] string json)
    {
        await _fileService.ImportTourFromJsonAsync(json);
        return Ok("Tour imported successfully");
    }
}