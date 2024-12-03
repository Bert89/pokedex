using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pokedex.Interfaces;
using Pokedex.Models;

namespace Pokedex.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<string> GetAsync(string pokemonName)
    {
        return await _distributedCache.GetStringAsync(pokemonName);
    }

    public async Task<string> SetAsync(PokemonModel pokemonModel)
    {
        var serializedPokemon = JsonConvert.SerializeObject(pokemonModel);
        await _distributedCache.SetStringAsync(pokemonModel.Name, serializedPokemon);
        return pokemonModel.Name;
    }
}