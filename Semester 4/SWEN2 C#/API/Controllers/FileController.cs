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
    public async Task<FileResult> GetSummaryReport()
    {
        var tours = await _tourService.GetAllToursAsync();
        var report = await _fileService.GenerateSummaryReportAsync(tours);
        return File(report, "application/pdf", "SummaryReport.pdf");
    }

    [ApiMethodDecorator]
    [HttpGet("tour/{tourId:guid}")]
    [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetTourReport(Guid tourId)
    {
        var report = await _fileService.GenerateTourReportAsync(tourId);
        return File(report, "application/pdf", $"TourReport_{tourId}.pdf");
    }

    [ApiMethodDecorator]
    [HttpGet("export/{tourId:guid}")]
    [ProducesResponseType(typeof(Tour), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> ExportTourToJson(Guid tourId)
    {
        var tourDomain = await _fileService.ExportTourToJsonAsync(tourId);
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
    public async Task<IActionResult> ImportTourFromJson([FromBody] string json)
    {
        await _fileService.ImportTourFromJsonAsync(json);
        return Ok("Tour imported successfully");
    }
}
