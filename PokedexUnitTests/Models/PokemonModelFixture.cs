using NUnit.Framework.Legacy;
using Pokedex.Models;


namespace PokedexUnitTests.Models
{
    public class PokemonModelTests
    {
        [Test]
        public void PokemonModel_DefaultValues_SetFields()
        {
            // Arrange
            var pokemon = new PokemonModel();

            // Act & Assert
            ClassicAssert.IsNull(pokemon.Name);
            ClassicAssert.IsNull(pokemon.Description);
            ClassicAssert.IsNull(pokemon.Habitat);
            ClassicAssert.IsFalse(pokemon.IsLegendary);
        }

        [Test]
        public void PokemonModel_SetValues_AreAssignedCorrectly()
        {
            // Arrange
            var pokemon = new PokemonModel
            {
                Name = "myPokemon",
                Description = "Desc",
                Habitat = "Habitat",
                IsLegendary = true
            };

            // Act & Assert
            ClassicAssert.AreEqual("myPokemon", pokemon.Name);
            ClassicAssert.AreEqual("Desc", pokemon.Description);
            ClassicAssert.AreEqual("Habitat", pokemon.Habitat);
            ClassicAssert.IsTrue(pokemon.IsLegendary);
        }

        
    }
}