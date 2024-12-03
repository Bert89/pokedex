using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework.Legacy;
using Pokedex.Models;
using Pokedex.Services;
using System.Text;

namespace PokedexUnitTests.Services;

[TestFixture]
public class CacheServiceFixture
{
    private CacheService _sut;
    private Mock<IDistributedCache> _distributedCache;

    [SetUp]
    public void Setup()
    {
        _distributedCache = new Mock<IDistributedCache>();

        _sut = new CacheService(_distributedCache.Object); 
    }

    [Test]
    public void CreateModel_GetAsync_ShouldReturnPokemonFromCache()
    {
        // Arrange
        var pokemonName = "myPoke";
        
        // Act
        var result = _sut.GetAsync(pokemonName).Result;

        // Assert
        _distributedCache.Verify(x => x.GetAsync(pokemonName, default), Times.Once);
    }

    [Test]
    public void CreateModel_GetAsync_ShouldSetPokemonInCache()
    {
        // Arrange
        var pokemonModel = new PokemonModel
        {
            Name = "myPoke",
            Description = "MyDesc",
            Habitat = "MyHabitat",
            IsLegendary = true
        };
        var serializedPokemon = JsonConvert.SerializeObject(pokemonModel);

        // Act
        var result = _sut.SetAsync(pokemonModel).Result;

        //Assert
        ClassicAssert.AreEqual("myPoke", result);
        _distributedCache.Verify(
            x => x.SetAsync("myPoke",
                Encoding.UTF8.GetBytes(serializedPokemon),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

}