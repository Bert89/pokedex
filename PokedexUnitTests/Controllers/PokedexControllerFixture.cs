using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using Pokedex.Controllers;
using Pokedex.Interfaces;
using Pokedex.Models;

namespace PokedexUnitTests.Controllers;

[TestFixture]
public class PokedexControllerFixture
{
    private Mock<ILogger<PokedexController>> _logger;
    private Mock<IPokedexService> _pokedexService;
    private PokedexController _sut;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<PokedexController>>();
        _pokedexService = new Mock<IPokedexService>();
        _sut = new PokedexController(_logger.Object, _pokedexService.Object);
    }

    [Test]
    public void GetPokemon_ShouldReturnOkResult_WhenPokemonIsFound()
    {
        // Arrange
        var pokemon = new PokemonModel
        {
            Name = "myPoke",
            Habitat = "myHB",
            IsLegendary = true,
            Description = "DD",
        };

        _pokedexService
            .Setup(service => service.GetPokemonAsync("myPoke"))
            .ReturnsAsync(pokemon);

        // Act
        var result = _sut.GetPokemon("myPoke").Result;

        // Assert
        var objectResult = result.Result as OkObjectResult;
        ClassicAssert.IsNotNull(objectResult);
        ClassicAssert.AreEqual(200, objectResult.StatusCode);

        var objectValue = objectResult.Value as PokemonModel;
        ClassicAssert.IsNotNull(objectValue);
        ClassicAssert.AreEqual("myPoke", objectValue.Name);
        ClassicAssert.AreEqual("DD", objectValue.Description);
        ClassicAssert.AreEqual("myHB", objectValue.Habitat);
        ClassicAssert.IsTrue(objectValue.IsLegendary);
    }

    [Test]
    public void GetPokemon_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var exception = new Exception("Pokemon not found");

        _pokedexService
            .Setup(service => service.GetPokemonAsync("myPoke"))
            .ThrowsAsync(exception);

        // Act
        var result = _sut.GetPokemon("myPoke").Result;

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        ClassicAssert.IsNotNull(badRequestResult);
        ClassicAssert.AreEqual(400, badRequestResult.StatusCode);

        var errorResponse = badRequestResult.Value as ErrorResponse;
        ClassicAssert.IsNotNull(errorResponse);
        ClassicAssert.AreEqual("Pokemon not found", errorResponse.Message);
        ClassicAssert.AreEqual(exception.Message, errorResponse.Details);

        _logger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, t) => state.ToString().Contains(exception.Message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public void GetTranslatedPokemon_ShouldReturnOkResult_WhenPokemonIsTranslated()
    {
        // Arrange
        var pokemon = new PokemonModel
        {
            Name = "myPoke",
            Habitat = "myHB",
            IsLegendary = true,
            Description = "DD",
        };
        var translatedPokemon = new PokemonModel
        {
            Name = "myPoke",
            Habitat = "myHB",
            IsLegendary = true,
            Description = "TranslatedDD",
        };

        _pokedexService
            .Setup(service => service.GetTranslatedPokemonAsync("myPoke"))
            .ReturnsAsync(translatedPokemon);

        // Act
        var result = _sut.GetTranslatedPokemon("myPoke").Result;

        // Assert
        var okResult = result.Result as OkObjectResult;
        ClassicAssert.IsNotNull(okResult);
        ClassicAssert.AreEqual(200, okResult.StatusCode);

        var returnValue = okResult.Value as PokemonModel;
        ClassicAssert.IsNotNull(returnValue);
        ClassicAssert.AreEqual("myPoke", returnValue.Name);
    }

    [Test]
    public async Task GetTranslatedPokemon_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var exception = new Exception("Pokemon not found");

        _pokedexService
            .Setup(service => service.GetTranslatedPokemonAsync("myPoke"))
            .ThrowsAsync(exception);

        // Act
        var result = await _sut.GetTranslatedPokemon("myPoke");

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        ClassicAssert.IsNotNull(badRequestResult);
        ClassicAssert.AreEqual(400, badRequestResult.StatusCode);

        var errorResponse = badRequestResult.Value as ErrorResponse;
        ClassicAssert.IsNotNull(errorResponse);
        ClassicAssert.AreEqual("Pokemon not found", errorResponse.Message);
        ClassicAssert.AreEqual(exception.Message, errorResponse.Details);
    }
}