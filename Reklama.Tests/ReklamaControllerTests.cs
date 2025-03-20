using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

//using Reklama.Controllers;
//using Reklama.Models;
//using Reklama.Services;






public class ReklamaControllerTests
{
    private readonly ReklamaController _controller;
    private readonly Mock<IReklamaService> _mockReklamaService;
    private readonly Mock<ILogger<ReklamaController>> _mockLogger;

    public ReklamaControllerTests()
    {
        _mockReklamaService = new Mock<IReklamaService>();
        _mockLogger = new Mock<ILogger<ReklamaController>>();
        _controller = new ReklamaController(_mockReklamaService.Object, _mockLogger.Object);
    }

    [Fact]
    public void LoadReklamas_ReturnsBadRequest_WhenFilePathIsInvalid()
    {
        var request = new FilePathRequest { FilePath = "invalid/path.txt"};

        var result = _controller.LoadReklamas(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("The file path is invalid or file does not exist.", badRequestResult.Value);
    }

    [Fact]
    public void LoadReklamas_ReturnsOk_WhenFilePathIsValid()
    {
        var filePath = Path.Combine("/app", "Data", "reklamas.txt");

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(filePath, "Test content");
        var request = new FilePathRequest { FilePath = filePath };

        _mockReklamaService.Setup(s => s.LoadReklamasFromFile(filePath));

        var result = _controller.LoadReklamas(request);
        if (result != null)
        {
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Data loaded successfully.", ((dynamic)okResult.Value).Message);
        }

        File.Delete(filePath);
    }
}
