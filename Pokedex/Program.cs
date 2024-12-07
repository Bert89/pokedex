using Pokedex.Interfaces;
using Pokedex.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPokedexService, PokedexService>();
builder.Services.AddSingleton<IPokemonApiClient, PokemonApiClient>();
builder.Services.AddSingleton<IYodaTranslatorService, YodaFunTranslatorService>();
builder.Services.AddSingleton<IShakespeareTranslatorService, ShakespeareFunTranslatorService>();
builder.Services.AddSingleton<ITranslatorServiceFactory, TranslatorServiceFactory>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
