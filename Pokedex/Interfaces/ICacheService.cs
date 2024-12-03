using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface ICacheService
{
    Task<string> GetAsync(string pokemonName);
    Task<string> SetAsync(PokemonModel pokemonModel);
}