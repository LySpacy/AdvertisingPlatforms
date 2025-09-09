namespace AdvertisingPlatforms.Domain.Interfaces;

public interface IAdPlatformRepository
{
    /// <summary>
    /// Загрузка данных из строк
    /// </summary>
    /// <param name="lines">Газеты с их локациями работы</param>
    void LoadFromLines(IEnumerable<string> lines);
    
    /// <summary>
    /// Поиск газет по локации
    /// </summary>
    /// <param name="location">индекс локации</param>
    /// <returns>Список газет</returns>
    List<string> FindPlatforms(string location);
}