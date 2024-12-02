using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface ITranslatorServiceFactory
{
    ITranslatorService Create(PokemonModel pokemonModel);
}