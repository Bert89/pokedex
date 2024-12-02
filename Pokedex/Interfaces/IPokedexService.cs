using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface IPokedexService
{
    /// <summary>
    /// Asynchronously retrieves translated info about a Pokemon based on its name.
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing a <see cref="PokemonModel"/> with the Pokemon's data.</returns>
    Task<PokemonModel> GetPokemonAsync(string pokemonName);

    /// <summary>
    /// Asynchronously retrieves info about a Pokemon based on its name.
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing a <see cref="PokemonModel"/> with the Pokemon's data and translated description.</returns>
    Task<PokemonModel> GetTranslatedPokemonAsync(string pokemonName);
}