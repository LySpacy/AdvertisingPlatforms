using AdvertisingPlatforms.Domain.DTO;
using Microsoft.AspNet.Http;

namespace AdvertisingPlatforms.Domain.Interfaces;

public interface IAdPlatformService
{
    /// <summary>
    /// Асинхронная выгрузка строк из файла
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task LoadAsync(IFormFile file, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Поиск газет по индексу локации
    /// </summary>
    /// <param name="location">Индекс локации</param>
    /// <returns>Список газет</returns>
    IEnumerable<AdPlatform> Search(string location);
}