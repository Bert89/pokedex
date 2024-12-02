using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface IPokedexService
{
    Task<PokemonModel> GetPokemonAsync(string pokemonName);
    Task<PokemonModel> GetTranslatedPokemonAsync(string pokemonName);
}