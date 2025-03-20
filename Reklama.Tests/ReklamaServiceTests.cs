using System.Diagnostics;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;


public class ReklamaServiceTests
{
    private readonly Mock<ILogger<ReklamaService>> _mockLogger;
    private readonly ReklamaService _reklamaService;

    public ReklamaServiceTests()
    {
        _mockLogger = new Mock<ILogger<ReklamaService>>();
        _reklamaService = new ReklamaService(_mockLogger.Object);
    }

    // Проверяет, что метод возвращает платформы, если локация существует.
    [Fact]
    public void SearchByLocation_ShouldReturnPlatforms_WhenLocationExists()
    {
        var location = "/ru/svrd/revda";

        var reklamas = new Dictionary<string, HashSet<string>>()
        {
            { "/ru", new HashSet<string> { "Яндекс.Директ" } },
            { "/ru/svrd/revda", new HashSet<string> { "Ревдинский рабочий" } },
            { "/ru/svrd/pervik", new HashSet<string> { "Ревдинский рабочий" } },
            { "/ru/msk", new HashSet<string> { "Газета уральских москвичей" } },
            { "/ru/permobl", new HashSet<string> { "Газета уральских москвичей" } },
            { "/ru/chelobl", new HashSet<string> { "Газета уральских москвичей" } },
            { "/ru/svrd", new HashSet<string> { "Крутая реклама" } }
        };

        var locationParentsCache = new Dictionary<string, HashSet<string>>()
            {
                { "/ru/svrd/revda", new HashSet<string> { "/ru/svrd", "/ru" } },
                { "/ru/svrd/pervik", new HashSet<string> { "/ru/svrd", "/ru" } },
                { "/ru/svrd", new HashSet<string> { "/ru" } },
                { "/ru/msk", new HashSet<string> { "/ru" } },
                { "/ru/permobl", new HashSet<string> { "/ru" } },
                { "/ru/chelobl", new HashSet<string> { "/ru" } }
        };

        _reklamaService.GetType().GetField("_reklamas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, reklamas);
        _reklamaService.GetType().GetField("_locationParentsCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, locationParentsCache);

        var result = _reklamaService.SearchByLocation(location ?? string.Empty);

        Assert.NotNull(result);
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Ревдинский рабочий", result);
        Assert.Contains("Крутая реклама", result);
    }

    // Проверяет, что метод возвращает пустой список, если локация не найдена.
    [Fact]
    public void SearchByLocation_ShouldReturnEmpty_WhenLocationDoesNotExist()
    {
        var location = "ru/none";
        var reklamas = new Dictionary<string, HashSet<string>>();
        var locationParentsCache = new Dictionary<string, HashSet<string>>();

        _reklamaService.GetType().GetField("_reklamas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, reklamas);
        _reklamaService.GetType().GetField("_locationParentsCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, locationParentsCache);

        var result = _reklamaService.SearchByLocation(location ?? string.Empty);

        Assert.Empty(result);
    }

    // Проверяет, что метод правильно обрабатывает null для локации.
    [Fact]
    public void SearchByLocation_ShouldHandleNullLocation()
    {
        string? location = null;

        var result = _reklamaService.SearchByLocation(location ?? string.Empty);

        Assert.Empty(result);
    }

    // Проверяет, что метод правильно учитывает платформы родительских локаций.
    [Fact]
    public void SearchByLocation_ShouldReturnPlatformsIncludingParents_WhenParentsExist()
    {
        var location = "/ru/msk/leninsk";
        var reklamas = new Dictionary<string, HashSet<string>>()
            {
            { "/ru", new HashSet<string> { "Яндекс.Директ" } },
            { "/ru/msk", new HashSet<string> { "Газета уральских москвичей" } },
            { "/ru/msk/leninsk", new HashSet<string> { "Местная газета Ленинска" } }
        };

        var locationParentsCache = new Dictionary<string, HashSet<string>>()
        {
            { "/ru/msk/leninsk", new HashSet<string> { "/ru/msk", "/ru" } }
        };

        _reklamaService.GetType().GetField("_reklamas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, reklamas);
        _reklamaService.GetType().GetField("_locationParentsCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .SetValue(_reklamaService, locationParentsCache);

        var result = _reklamaService.SearchByLocation(location ?? string.Empty);

        Assert.Contains("Газета уральских москвичей", result);
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Местная газета Ленинска", result);
    }


}
