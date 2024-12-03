using Newtonsoft.Json;
using Pokedex.Models;
using System.Text;

namespace Pokedex.Services;

public abstract class FunTranslatorServiceBase
{
    protected readonly ILogger<FunTranslatorServiceBase> TranslatorLogger;
    private readonly string _translator;
    protected readonly HttpClient HttpClient;
    private const string BaseAddress = "https://api.funtranslations.com/translate/";

    protected FunTranslatorServiceBase(ILogger<FunTranslatorServiceBase> translatorLogger, string translator)
    {
        TranslatorLogger = translatorLogger;
        _translator = translator;
        HttpClient = new HttpClient();
    }

    public virtual async Task<FunTranslationResponse> TranslateAsync(string description) 
    {
        try
        {
            var payload = new { text = description };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync($"{BaseAddress}{_translator}", jsonContent);
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