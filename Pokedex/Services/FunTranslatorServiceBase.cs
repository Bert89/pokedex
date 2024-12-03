using System.Text;
using Newtonsoft.Json;
using Pokedex.Models;

namespace Pokedex.Services;

public abstract class FunTranslatorServiceBase
{
    private const string BaseAddress = "https://api.funtranslations.com/translate/";
    private readonly HttpClient _httpClient;
    private readonly string _translator;
    protected readonly ILogger<FunTranslatorServiceBase> TranslatorLogger;

    protected FunTranslatorServiceBase(ILogger<FunTranslatorServiceBase> translatorLogger, string translator)
    {
        TranslatorLogger = translatorLogger;
        _translator = translator;
        _httpClient = new HttpClient();
    }

    public virtual async Task<FunTranslationResponse> TranslateAsync(string description)
    {
        try
        {
            var payload = new { text = description };
            var jsonContent =
                new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseAddress}{_translator}", jsonContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FunTranslationResponse>(responseContent);
        }
        catch (Exception ex)
        {
            TranslatorLogger.LogError(ex, ex.Message, ex.InnerException);
            throw;
        }
    }
}