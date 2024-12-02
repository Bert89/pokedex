using Pokedex.Interfaces;
using Pokedex.Models;

namespace Pokedex.Services;

public class PokedexService : IPokedexService
{
    private readonly ILogger<PokedexService> _logger;

    public PokedexService(ILogger<PokedexService> logger)
    {
        _logger = logger;
    }


    public Task<PokemonModel> GetPokemonAsync(string pokemonName)
    {
        throw new NotImplementedException();
    }

    public Task<PokemonModel> GetTranslatedPokemonAsync(string pokemonName)
    {
        throw new NotImplementedException();
    }
}