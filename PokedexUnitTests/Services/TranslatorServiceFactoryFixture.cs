using Moq;
using NUnit.Framework.Legacy;
using Pokedex.Interfaces;
using Pokedex.Models;
using Pokedex.Services;

namespace PokedexUnitTests.Services;

[TestFixture]
public class TranslatorServiceFactoryFixture
{
    private TranslatorServiceFactory _sut;
    private Mock<IYodaTranslatorService> _yodaTranslatorService;
    private Mock<IShakespeareTranslatorService> _shakespeareTranslatorService;

    [SetUp]
    public void Setup()
    {
        _yodaTranslatorService = new Mock<IYodaTranslatorService>();
        _shakespeareTranslatorService = new Mock<IShakespeareTranslatorService>();

        _sut = new TranslatorServiceFactory(_yodaTranslatorService.Object, _shakespeareTranslatorService.Object); 
    }

    [Test]
    public void CreateEmptyPokemon_CreateTranslator_ReturnsShakespeareTranslator()
    {
        // Arrange
        var pokemon = new PokemonModel();
        
        // Act
        var result = _sut.Create(pokemon);

        // Assert
        ClassicAssert.AreEqual(result, _shakespeareTranslatorService.Object);
    }

    [Test]
    public void NullPokemon_CreateTranslator_ReturnsShakespeareTranslator()
    {
        // Arrange

        // Act
        var result = _sut.Create(null);

        // Assert
        ClassicAssert.AreEqual(result, _shakespeareTranslatorService.Object);
    }

    [Test]
    public void CreateLegendaryPokemon_CreateTranslator_ReturnsYodaTranslator()
    {
        // Arrange
        var pokemon = new PokemonModel()
        {
            IsLegendary = true
        };

        // Act
        var result = _sut.Create(pokemon);

        // Assert
        ClassicAssert.AreEqual(result, _yodaTranslatorService.Object);
    }

    [Test]
    public void CreateCavePokemon_CreateTranslator_ReturnsYodaTranslator()
    {
        // Arrange
        var pokemon = new PokemonModel
        {
            Habitat = "cave"
        };

        // Act
        var result = _sut.Create(pokemon);

        // Assert
        ClassicAssert.AreEqual(result, _yodaTranslatorService.Object);
    }

}