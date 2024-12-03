using Pokedex.Interfaces;

namespace Pokedex.Services;

public class YodaFunTranslatorService : FunTranslatorServiceBase, IYodaTranslatorService
{
    public YodaFunTranslatorService(ILogger<YodaFunTranslatorService> logger) : base(logger, "yoda.json")
    {
    }
}