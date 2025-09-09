using Microsoft.AspNetCore.Http.Abstractions;
using AdvertisingPlatforms.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.API.Controllers;

[ApiController]
[Route("api/adplatforms")]
public class AdPlatformController : ControllerBase
{
    private readonly IAdPlatformService _service;

    public AdPlatformController(IAdPlatformService service)
    {
        _service = service;
    }

    [HttpPost("load")]
    public async Task<IActionResult> Load(IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл пуст");

        await _service.LoadAsync(file, cancellationToken);
        return Ok("Файл загружен");
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string location)
    {
        var result = _service.Search(location);
        return Ok(result);
    }
}