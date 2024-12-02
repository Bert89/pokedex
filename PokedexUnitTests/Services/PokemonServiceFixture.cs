using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using PokeApiNet;
using Pokedex.Interfaces;
using Pokedex.Services;

namespace PokedexUnitTests.Services
{
    public class PokedexServiceFixture
    {
        private PokedexService _sut;
        private Mock<ILogger<PokedexService>> _logger;
        private Mock<IPokemonApiClient> _pokemonApiClient;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PokedexService>>();
            _pokemonApiClient = new Mock<IPokemonApiClient>();
            _sut = new PokedexService(_logger.Object, _pokemonApiClient.Object);
        }

        [Test]
        public void SingleEngDescription_GetPokemonAsync_ReturnsExpectedPokemonModel()
        {
            // Arrange
            var species = new PokemonSpecies
            {
                Name = "myPokemon",
                Habitat = new NamedApiResource<PokemonHabitat> { Name = "HB" },
                IsLegendary = false,
                FlavorTextEntries =
                [
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionIT",
                        Language = new NamedApiResource<Language> { Name = "it" }
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionEN",
                        Language = new NamedApiResource<Language> { Name = "en" }
                    }
                ]
            };

            _pokemonApiClient
                .Setup(client => client.GetResourceAsync<PokemonSpecies>("myPokemon"))
                .ReturnsAsync(species);

            // Act
            var result = _sut.GetPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsFalse(result.IsLegendary);
        }

        [Test]
        public void MultipleEngDescription_GetPokemonAsync_ReturnsFirstDescriptionPokemonModel()
        {
            // Arrange
            var species = new PokemonSpecies
            {
                Name = "myPokemon",
                Habitat = new NamedApiResource<PokemonHabitat> { Name = "HB" },
                IsLegendary = false,
                FlavorTextEntries =
                [
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionIT",
                        Language = new NamedApiResource<Language> { Name = "it" }
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionEN1",
                        Language = new NamedApiResource<Language> { Name = "en" }
                    },
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionEN2",
                        Language = new NamedApiResource<Language> { Name = "en" }
                    }
                ]
            };

            _pokemonApiClient
                .Setup(client => client.GetResourceAsync<PokemonSpecies>("myPokemon"))
                .ReturnsAsync(species);

            // Act
            var result = _sut.GetPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN1", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsFalse(result.IsLegendary);
        }

        [Test]
        public void NoEngDescription_GetPokemonAsync_ReturnsNoDescriptionPokemonModel()
        {
            // Arrange
            var species = new PokemonSpecies
            {
                Name = "myPokemon",
                Habitat = new NamedApiResource<PokemonHabitat> { Name = "HB" },
                IsLegendary = false,
                FlavorTextEntries =
                [
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "descriptionIT",
                        Language = new NamedApiResource<Language> { Name = "it" }
                    }
                ]
            };

            _pokemonApiClient
                .Setup(client => client.GetResourceAsync<PokemonSpecies>("myPokemon"))
                .ReturnsAsync(species);

            // Act
            var result = _sut.GetPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.IsEmpty(result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsFalse(result.IsLegendary);
        }

        [Test]
        public void EngDescriptionWithSpecialChars_GetPokemonAsync_ReturnsClearDescriptionPokemonModel()
        {
            // Arrange
            var species = new PokemonSpecies
            {
                Name = "myPokemon",
                Habitat = new NamedApiResource<PokemonHabitat> { Name = "HB" },
                IsLegendary = false,
                FlavorTextEntries =
                [
                    new PokemonSpeciesFlavorTexts
                    {
                        FlavorText = "description\trow1\nrow2",
                        Language = new NamedApiResource<Language> { Name = "en" }
                    }
                ]
            };

            _pokemonApiClient
                .Setup(client => client.GetResourceAsync<PokemonSpecies>("myPokemon"))
                .ReturnsAsync(species);

            // Act
            var result = _sut.GetPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("description row1 row2", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsFalse(result.IsLegendary);
        }

        [Test]
        public void GetPokemonAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("ExcMessage");

            _pokemonApiClient
                .Setup(client => client.GetResourceAsync<PokemonSpecies>("fakePokemon"))
                .Throws(exception);

            // Act and Assert
            Assert.Throws<AggregateException>(() => _ = _sut.GetPokemonAsync("fakePokemon").Result, "ExcMessage");

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains(exception.Message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}