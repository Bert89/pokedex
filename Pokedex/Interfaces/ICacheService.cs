using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface ICacheService
{
    Task<string> GetAsync(string pokemonName);
    Task SetAsync(PokemonModel pokemonModel);
}