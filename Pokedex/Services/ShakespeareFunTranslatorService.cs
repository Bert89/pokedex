using Pokedex.Interfaces;

namespace Pokedex.Services;

public class ShakespeareFunTranslatorService : FunTranslatorServiceBase, IShakespeareTranslatorService
{
    public ShakespeareFunTranslatorService(ILogger<ShakespeareFunTranslatorService> logger) : base(logger, "shakespeare.json")
    {
    }
}