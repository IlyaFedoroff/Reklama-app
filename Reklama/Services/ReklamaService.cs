
public class ReklamaService : IReklamaService
{
    private readonly Dictionary<string, HashSet<string>> _reklamas;
    private  readonly Dictionary<string, HashSet<string>> _locationParentsCache;
    private readonly ILogger<ReklamaService> _logger;

    public ReklamaService(ILogger<ReklamaService> logger)
    {
        _logger = logger;
        _reklamas = new();
        _locationParentsCache = new Dictionary<string, HashSet<string>>();
    }

    public void LoadReklamasFromFile(string filePath)
    {
        _logger.LogInformation("Loading reklamas from file: {FilePath}", filePath);

        _reklamas.Clear();
        _locationParentsCache.Clear();

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("The file path is invalid or file does not exist: {FilePath}", filePath);
            return;
        }

        var lines = File.ReadAllLines(filePath);
        _logger.LogInformation("Loaded {LineCount} lines from file: {FilePath}", lines.Length, filePath);

        foreach (var line in lines)
        {
            var parts = line.Trim().Split(':');
            if (parts.Length != 2)
            {
                _logger.LogWarning("Skipping invalid line: {Line}", line);
                continue;
            }

            var platform = parts[0].Trim();
            var locations = parts[1].Split(',');

            foreach (var location in locations)
            {
                var loc = location.Trim();
                if (string.IsNullOrWhiteSpace(loc))
                {
                    _logger.LogWarning("Skipping empty or invalid location: {Location}", location);
                    continue;
                }

                if (!_reklamas.ContainsKey(loc))
                {
                    _reklamas[loc] = new HashSet<string>();
                    _logger.LogInformation("Added new location: {Location}", loc);
                }

                if (!_reklamas[loc].Contains(platform))
                {
                    _reklamas[loc].Add(platform);
                    _logger.LogInformation("Added platform {Platform} to location {Location}", platform, loc);
                }

                var parentLocations = GetParentLocations(loc);
                _locationParentsCache[loc] = new HashSet<string>(parentLocations);

                foreach (var parentLocation in parentLocations)
                {
                    if (!_locationParentsCache.ContainsKey(parentLocation))
                    {
                        _locationParentsCache[parentLocation] = new HashSet<string>();
                    }
                    _locationParentsCache[parentLocation].Add(loc);
                }
            }
        }

        _logger.LogInformation("Finished loading reklamas from file.");
        _logger.LogInformation("Current reklamas data:");
        foreach (var kvp in _reklamas)
        {
            _logger.LogInformation("Location: {Location}, Platforms: {Platforms}", kvp.Key, string.Join(", ", kvp.Value));
        }
    }



    public List<string> SearchByLocation(string? location)
    {
        //_logger.LogInformation("Searching platforms for location: {Location}", location);

        if (string.IsNullOrEmpty(location))
        {
            _logger.LogWarning("Location field is either null or empty.");
            return new List<string>();
        }

        var result = new HashSet<string>();

        if (_reklamas.TryGetValue(location, out var platforms))
        {
            result.UnionWith(platforms);
            // _logger.LogInformation("Found {PlatformCount} platform(s) for location: {Location}: {Platforms}",
            //     platforms.Count, location, string.Join(", ", platforms));
        }

        if (_locationParentsCache.TryGetValue(location, out var parentLocations))
        {
            foreach (var parentLocation in parentLocations)
            {
                if (_reklamas.TryGetValue(parentLocation, out var parentPlatforms))
                {
                    result.UnionWith(parentPlatforms);
                    // _logger.LogInformation("Found {PlatformCount} platform(s) for parent location: {ParentLocation}: {Platforms}",
                    //     parentPlatforms.Count, parentLocation, string.Join(", ", parentPlatforms));
                }
            }

        }
        //_logger.LogInformation("Found {PlatformCount} platform(s) for location: {Location} and its parents", result.Count, location);
        return result.ToList();

    }

    private List<string> GetParentLocations(string location)
    {
        var parentLocations = new List<string>();
        var currentLocation = location;

        while (!string.IsNullOrEmpty(currentLocation))
        {
            var lastSlashIndex = currentLocation.LastIndexOf('/');
            if (lastSlashIndex == -1)
            {
                break;
            }

            currentLocation = currentLocation.Substring(0, lastSlashIndex);
            parentLocations.Add(currentLocation);
        }

        return parentLocations;
    }
}
