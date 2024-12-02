using Pokedex.Models;

namespace Pokedex.Interfaces;

public interface ITranslatorService
{
    Task<FunTranslationResponse> TranslateAsync(string description);
}