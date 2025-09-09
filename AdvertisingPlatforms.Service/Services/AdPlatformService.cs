using Microsoft.AspNet.Http;

namespace AdvertisingPlatforms.Service.Services;

public class AdPlatformService : IAdPlatformService
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IAdPlatformRepository _repo;
    public AdPlatformService(IAdPlatformRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Асинхронное чтение файла
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task LoadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var lines = new List<string>();
        using var reader = new StreamReader(file.OpenReadStream());
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);
        }

        _repo.LoadFromLines(lines);
    }

    /// <summary>
    /// Поиск рекламных платформ по локации
    /// </summary>
    /// <param name="location">Локация</param>
    /// <returns>Список наименования рекламных платформ</returns>
    public List<string> Search(string location) => _repo.FindPlatforms(location);
}