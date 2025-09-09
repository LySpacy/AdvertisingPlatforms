using Microsoft.AspNet.Http;

namespace AdvertisingPlatforms.Service.Services;

public class AdPlatformService : IAdPlatformService
{
    private readonly IAdPlatformRepository _repo;

    public AdPlatformService(IAdPlatformRepository repo)
    {
        _repo = repo;
    }

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

    public List<string> Search(string location) => _repo.FindPlatforms(location);
}