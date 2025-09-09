namespace AdvertisingPlatforms.Domain.Models;

/// <summary>
///  Ветка локации
/// </summary>
public class LocationNode
{
    /// <summary>
    /// Наименование ветки "/{name}"
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Дети ветки
    /// </summary>
    public Dictionary<string, LocationNode> Children { get; } = new();
    
    /// <summary>
    /// Платформы подходящие к этой ветке
    /// </summary>
    public List<string> Platforms { get; } = new();
}
