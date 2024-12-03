using Newtonsoft.Json;
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
    private readonly ICacheService _cacheService;

    public PokedexService(ILogger<PokedexService> logger, 
        IPokemonApiClient pokemonApiClient,
        ITranslatorServiceFactory translatorServiceFactory, 
        ICacheService cacheService)
    {
        _logger = logger;
        _pokemonApiClient = pokemonApiClient;
        _translatorServiceFactory = translatorServiceFactory;
        _cacheService = cacheService;
    }

    /// <inheritdoc />
    public async Task<PokemonModel> GetPokemonAsync(string pokemonName)
    {
        try
        {
            var cachedPokemon = await _cacheService.GetAsync(pokemonName);
            if (!string.IsNullOrWhiteSpace(cachedPokemon))
            {
                _logger.LogInformation($"Cached data for {pokemonName}");
                return JsonConvert.DeserializeObject<PokemonModel>(cachedPokemon);
            }

            var pokemon = await _pokemonApiClient.GetResourceAsync<PokemonSpecies>(pokemonName);

            var pokemonModel = new PokemonModel
            {
                Name = pokemon.Name,
                Description = ReadDescription(pokemon),
                Habitat = pokemon.Habitat.Name,
                IsLegendary = pokemon.IsLegendary
            };

            await _cacheService.SetAsync(pokemonModel);
            return pokemonModel;
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