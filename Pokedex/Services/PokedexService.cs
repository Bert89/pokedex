using PokeApiNet;
using Pokedex.Interfaces;
using Pokedex.Models;
using System.Text.RegularExpressions;

namespace Pokedex.Services;

public class PokedexService : IPokedexService
{
    private readonly IPokemonApiClient _pokemonApiClient;
    private readonly ITranslatorServiceFactory _translatorServiceFactory;
    private readonly ILogger<PokedexService> _logger;

    public PokedexService(ILogger<PokedexService> logger, 
        IPokemonApiClient pokemonApiClient,
        ITranslatorServiceFactory translatorServiceFactory)
    {
        _logger = logger;
        _pokemonApiClient = pokemonApiClient;
        _translatorServiceFactory = translatorServiceFactory;
    }

    /// <inheritdoc />
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
            _logger.LogError(ex.Message);
            throw;
        }
    }


    /// <inheritdoc />
    public async Task<PokemonModel> GetTranslatedPokemonAsync(string pokemonName)
    {
        var pokemon = new PokemonModel();
        try
        {
            pokemon = await GetPokemonAsync(pokemonName);

            var translation = await _translatorServiceFactory
                .Create(pokemon)
                .TranslateAsync(pokemon.Description);

            if (translation?.Contents == null)
                throw new Exception("Impossible to read translation");

            pokemon.Description = translation.Contents.Translated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, ex.InnerException);
            pokemon.Error = $"Standard translation used due to error: {ex.Message}";
        }

        return pokemon;
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