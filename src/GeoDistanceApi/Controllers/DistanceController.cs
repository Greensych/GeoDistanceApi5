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
    public IActionResult Calculate([FromBody] DistanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (request?.From == null || request?.To == null)
            return BadRequest("Both coordinates required");

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating distance");
            return StatusCode(500, new ErrorResponse { Message = "Calculation failed" });
        }
    }
}
