using Microsoft.AspNetCore.Http.Abstractions;
using AdvertisingPlatforms.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.API.Controllers;

[ApiController]
[Route("api/adplatforms")]
public class AdPlatformController : ControllerBase
{
    private readonly IAdPlatformService _service;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="service">Сервис для работы с рекламными платформами.</param>
    public AdPlatformController(IAdPlatformService service)
    {
        _service = service;
    }

    /// <summary>
    /// Загрузка файла с данными рекламных платформ и их локаций.
    /// </summary>
    /// <param name="file">Файл для загрузки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Возвращает статус выполнения операции.</returns>
    [HttpPost("load")]
    public async Task<IActionResult> Load(IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл пуст");

        await _service.LoadAsync(file, cancellationToken);
        return Ok("Файл загружен");
    }

    /// <summary>
    /// Поиск рекламных платформ по указанной локации.
    /// </summary>
    /// <param name="location">Локация для поиска (например, "/ru/msk").</param>
    /// <returns>Список наименований подходящих рекламных платформ.</returns>
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string location)
    {
        var result = _service.Search(location);
        return Ok(result);
    }
}