using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using PokeApiNet;
using Pokedex.Interfaces;
using Pokedex.Models;
using Pokedex.Services;

namespace PokedexUnitTests.Services
{
    public class PokedexServiceFixture
    {
        private PokedexService _sut;
        private Mock<ILogger<PokedexService>> _logger;
        private Mock<IPokemonApiClient> _pokemonApiClient;
        private Mock<ITranslatorServiceFactory> _translatorServiceFactory;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PokedexService>>();
            _pokemonApiClient = new Mock<IPokemonApiClient>();
            _translatorServiceFactory = new Mock<ITranslatorServiceFactory>();
            _sut = new PokedexService(_logger.Object, 
                _pokemonApiClient.Object,
                _translatorServiceFactory.Object);
        }

        #region GetPokemonAsync

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
            ClassicAssert.IsNotNull(result.Name);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsFalse(result.IsLegendary);
            ClassicAssert.IsNull(result.Error);
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
            ClassicAssert.IsNull(result.Error);
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
            ClassicAssert.IsNull(result.Error);
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
            ClassicAssert.IsNull(result.Error);
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

        #endregion

        #region GetTranslatedPokemonAsync

        [Test]
        public void NoTranslationAvailable_GetTranslatedPokemonAsync_ReturnsDefaultDescription()
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

            var translatorService = new Mock<ITranslatorService>();
            _translatorServiceFactory
                .Setup(x => x.Create(
                    It.Is<PokemonModel>(y => y.Name == "myPokemon" &&
                                             y.Description == "descriptionEN" &&
                                             y.Habitat == "HB" &&
                                             !y.IsLegendary)))
                .Returns(translatorService.Object);

            // Act
            var result = _sut.GetTranslatedPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.AreEqual("Standard translation used due to error: Impossible to read translation", result.Error);
            ClassicAssert.IsFalse(result.IsLegendary);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Impossible to read translation")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        
        [Test]
        public void NoTranslationContentAvailable_GetTranslatedPokemonAsync_ReturnsDefaultDescription()
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

            var translatorService = new Mock<ITranslatorService>();
            var response = new FunTranslationResponse();
            translatorService
                .Setup(x => x.TranslateAsync("descriptionEN"))
                .ReturnsAsync(response);
            
            _translatorServiceFactory
                .Setup(x => x.Create(
                    It.Is<PokemonModel>(y => y.Name == "myPokemon" &&
                                             y.Description == "descriptionEN" &&
                                             y.Habitat == "HB" &&
                                             !y.IsLegendary)))
                .Returns(translatorService.Object);

            // Act
            var result = _sut.GetTranslatedPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.AreEqual("Standard translation used due to error: Impossible to read translation", result.Error);
            ClassicAssert.IsFalse(result.IsLegendary);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Impossible to read translation")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [Test]
        public void TranslationThrowsException_GetTranslatedPokemonAsync_ReturnsDefaultDescription()
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

            var translatorService = new Mock<ITranslatorService>();

            translatorService
                .Setup(x => x.TranslateAsync("descriptionEN"))
                .ThrowsAsync(new Exception("Translation Error"));

            _translatorServiceFactory
                .Setup(x => x.Create(
                    It.Is<PokemonModel>(y => y.Name == "myPokemon" &&
                                             y.Description == "descriptionEN" &&
                                             y.Habitat == "HB" &&
                                             !y.IsLegendary)))
                .Returns(translatorService.Object);

            // Act
            var result = _sut.GetTranslatedPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("descriptionEN", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.AreEqual("Standard translation used due to error: Translation Error", result.Error);
            ClassicAssert.IsFalse(result.IsLegendary);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Translation Error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }



        [Test]
        public void TranslationAvailable_GetTranslatedPokemonAsync_ReturnsTranslatedDescription()
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

            var translatorService = new Mock<ITranslatorService>();
            var response = new FunTranslationResponse()
            {
                Contents = new FunTranslationContents
                {
                    Translated = "translatedDesc"
                }
            };
            translatorService
                .Setup(x => x.TranslateAsync("descriptionEN"))
                .ReturnsAsync(response);

            _translatorServiceFactory
                .Setup(x => x.Create(
                    It.Is<PokemonModel>(y => y.Name == "myPokemon" &&
                                             y.Description == "descriptionEN" &&
                                             y.Habitat == "HB" &&
                                             !y.IsLegendary)))
                .Returns(translatorService.Object);

            // Act
            var result = _sut.GetTranslatedPokemonAsync("myPokemon").Result;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("myPokemon", result.Name);
            ClassicAssert.AreEqual("translatedDesc", result.Description);
            ClassicAssert.AreEqual("HB", result.Habitat);
            ClassicAssert.IsNull(result.Error);
            ClassicAssert.IsFalse(result.IsLegendary);
        }

        #endregion

    }
}