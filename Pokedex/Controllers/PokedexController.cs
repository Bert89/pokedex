using Microsoft.AspNetCore.Mvc;
using Pokedex.Interfaces;
using Pokedex.Models;

namespace Pokedex.Controllers;

[Route("pokemon")]
[ApiController]
public class PokedexController : ControllerBase
{
    private readonly ILogger<PokedexController> _logger;
    private readonly IPokedexService _pokedexService;

    public PokedexController(ILogger<PokedexController> logger, IPokedexService pokedexService)
    {
        _logger = logger;
        _pokedexService = pokedexService;
    }


    [HttpGet("{pokemonName}")]
    public async Task<ActionResult<PokemonModel>> GetPokemon(string pokemonName)
    {
        try
        {
            var pokemon = await _pokedexService.GetPokemonAsync(pokemonName);
            return Ok(pokemon);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message, ex.InnerException);
            return BadRequest(new ErrorResponse
            {
                Message = "Pokemon not found",
                Details = ex.Message
            });
        }
    }
    
    [HttpGet("translated/{pokemonName}")]
    public async Task<ActionResult<PokemonModel>> GetTranslatedPokemon(string pokemonName)
    {
        try
        {
            var translatedPokemon = await _pokedexService.GetTranslatedPokemonAsync(pokemonName);
            return Ok(translatedPokemon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, ex.InnerException);
            return BadRequest(new ErrorResponse
            {
                Message = "Pokemon not found",
                Details = ex.Message
            });
        }
    }
}