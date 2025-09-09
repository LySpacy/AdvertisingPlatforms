public class InMemoryAdPlatformRepository : IAdPlatformRepository
{
    
    /// <summary>
    /// Храним в памяти все ноды
    /// </summary>
    private volatile LocationNode _root = new LocationNode();

    /// <summary>
    /// Обрабатывает строки данных рекламных платформ с их индексами локаций
    /// </summary>
    /// <param name="lines">строки с рекламными платформами с их индексами локаций</param>
    public void LoadFromLines(IEnumerable<string> lines)
    {
        var newRoot = new LocationNode();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains(":"))
                continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2) continue;

            var platform = parts[0].Trim();
            if (string.IsNullOrEmpty(platform)) continue;

            var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var loc in locations)
            {
                var cleanLoc = loc.Trim();
                if (!cleanLoc.StartsWith("/")) continue;

                AddLocation(newRoot, cleanLoc, platform);
            }
        }

        _root = newRoot;
    }
    
    /// <summary>
    /// Поиск рекламных платформа по локации
    /// </summary>
    /// <param name="location">Локация</param>
    /// <returns>Список наименования платформ</returns>
    public List<string> FindPlatforms(string location)
    {
        if (string.IsNullOrWhiteSpace(location) || !location.StartsWith("/"))
            return new List<string>();

        var result = new List<string>();
        var segments = location.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var node = _root;
        foreach (var segment in segments)
        {
            if (node.Platforms.Count > 0)
                result.AddRange(node.Platforms);

            if (!node.Children.TryGetValue(segment, out node))
                break;
        }

        if (node != null && node.Platforms.Count > 0)
            result.AddRange(node.Platforms);

        return result.Distinct().ToList();
    }
    
    /// <summary>
    /// Добавление платформы с локацией в ноду
    /// </summary>
    /// <param name="root">Нода</param>
    /// <param name="location">Локация</param>
    /// <param name="platform">Наименование платформы</param>
    private void AddLocation(LocationNode root, string location, string platform)
    {
        var segments = location.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var node = root;
        foreach (var segment in segments)
        {
            if (!node.Children.TryGetValue(segment, out var child))
            {
                child = new LocationNode { Name = segment };
                node.Children[segment] = child;
            }
            node = child;
        }

        node.Platforms.Add(platform);
    }
}