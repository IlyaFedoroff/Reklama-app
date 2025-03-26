using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class ReklamaController : ControllerBase
{
    private readonly IReklamaService _reklamaService;
    private readonly ILogger<ReklamaController> _logger;

    public ReklamaController(IReklamaService reklamaService, ILogger<ReklamaController> logger)
    {
        _reklamaService = reklamaService;
        _logger = logger;
    }

    [HttpPost("load")]
    public IActionResult LoadReklamas([FromBody] FilePathRequest request)
    {

        _logger.LogInformation("Received file path: {FilePath}", request?.FilePath);

        if (string.IsNullOrWhiteSpace(request?.FilePath) || !System.IO.File.Exists(request.FilePath))
        {
            _logger.LogWarning("The file path is invalid or file does not exist: {FilePath}", request?.FilePath);
            return BadRequest("The file path is invalid or file does not exist.");
        }

        _logger.LogInformation("Loading reklamas from file: {FilePath}", request.FilePath);
        _reklamaService.LoadReklamasFromFileAsync(request.FilePath);
        _logger.LogInformation("Data loaded successfully from {FilePath}", request.FilePath);

        return Ok(new { Message = "Data loaded successfully." });
    }

    [HttpGet("search")]
    public IActionResult SearchReklamas([FromQuery] string location)
    {
        _logger.LogInformation("Received search request for location: {Location}", location);

        if (string.IsNullOrEmpty(location))
        {
            _logger.LogWarning("Location field is missing or empty.");
            return BadRequest("The location field is required.");
        }

        var result = _reklamaService.SearchByLocation(location);
        if (result.Count == 0)
        {
            _logger.LogWarning("No reklamas found for the given location: {Locattion}", location);
            return NotFound("No reklamas found for the given location.");
        }

        _logger.LogInformation("Found {ResultCount} reklamas for location: {Location}", result.Count, location);

        return Ok(result);
    }
}
