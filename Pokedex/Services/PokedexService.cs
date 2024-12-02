using PokeApiNet;
using Pokedex.Interfaces;
using Pokedex.Models;
using System.Text.RegularExpressions;

namespace Pokedex.Services;

public class PokedexService : IPokedexService
{
    private readonly IPokemonApiClient _pokemonApiClient;
    private readonly ILogger<PokedexService> _logger;

    public PokedexService(ILogger<PokedexService> logger, IPokemonApiClient pokemonApiClient)
    {
        _logger = logger;
        _pokemonApiClient = pokemonApiClient;
    }

    public async Task<PokemonModel> GetPokemonAsync(string pokemonName)
    {
        try
        {
            var pokemon = await _pokemonApiClient.GetResourceAsync<PokemonSpecies>(pokemonName);

            return new PokemonModel
            {
                Name = pokemon.Name,
                Description = ReadDescription(pokemon),
                Habitat = pokemon.Habitat.Name,
                IsLegendary = pokemon.IsLegendary
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, ex.InnerException);
            throw;
        }
    }


    public async Task<PokemonModel> GetTranslatedPokemonAsync(string pokemonName)
    {
        throw new NotImplementedException();
    }

    private string ReadDescription(PokemonSpecies species)
    {
        var engDescription = species.FlavorTextEntries
            .FirstOrDefault(x => x.Language.Name == "en" && !string.IsNullOrWhiteSpace(x.FlavorText));

        return engDescription != default
            ? Regex.Replace(engDescription.FlavorText, @"\t|\n|\r|\f", " ")
            : string.Empty;
    }
}