using Pokedex.Interfaces;
using Pokedex.Models;

namespace Pokedex.Services;

public class TranslatorServiceFactory : ITranslatorServiceFactory
{
    private readonly IShakespeareTranslatorService _shakespeareTranslatorService;
    private readonly IYodaTranslatorService _yodaTranslatorService;

    public TranslatorServiceFactory(IYodaTranslatorService yodaTranslatorService,
        IShakespeareTranslatorService shakespeareTranslatorService)
    {
        _yodaTranslatorService = yodaTranslatorService;
        _shakespeareTranslatorService = shakespeareTranslatorService;

    }


    public ITranslatorService Create(PokemonModel pokemonModel)
    {
        return pokemonModel.IsLegendary || pokemonModel.Habitat == "cave"
            ? _yodaTranslatorService
            : _shakespeareTranslatorService;
    }
}