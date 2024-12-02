using PokeApiNet;

namespace Pokedex.Interfaces;

public interface IPokemonApiClient
{
    Task<T> GetResourceAsync<T>(string name) where T : NamedApiResource;

}