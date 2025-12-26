using GeoDistanceApi.Models;
using GeoDistanceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoDistanceApi.Controllers;

[ApiController]
[Route("api/distance")]
public class DistanceController : ControllerBase
{
    private readonly IDistanceService _service;
    private readonly ILogger<DistanceController> _logger;

    public DistanceController(IDistanceService service, ILogger<DistanceController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(DistanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] DistanceRequest? request)
    {
        if (request?.From == null || request?.To == null)
        {
            return BadRequest(new ErrorResponse 
            { 
                Message = "Both 'from' and 'to' coordinates are required" 
            });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Coordinates must be in range: Latitude[-90,90], Longitude[-180,180]"
            });
        }

        try
        {
            var distance = _service.CalculateDistance(request.From, request.To);
            return Ok(new DistanceResponse
            {
                DistanceKm = Math.Round(distance, 3),
                From = request.From,
                To = request.To
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid arguments");
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            return StatusCode(500, new ErrorResponse 
            { 
                Message = "Internal error calculating distance",
                Details = ex.Message 
            });
        }
    }
}
