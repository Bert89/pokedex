using NUnit.Framework.Legacy;
using Pokedex.Models;


namespace PokedexUnitTests.Models;

[TestFixture]
public class PokemonModelFixture
{
    [Test]
    public void PokemonModel_DefaultValues_SetFields()
    {
        // Arrange
        var sut = new PokemonModel();

        // Act & Assert
        ClassicAssert.IsNull(sut.Name);
        ClassicAssert.IsNull(sut.Description);
        ClassicAssert.IsNull(sut.Habitat);
        ClassicAssert.IsFalse(sut.IsLegendary);
    }

    [Test]
    public void PokemonModel_SetValues_AreAssignedCorrectly()
    {
        // Arrange
        var sut = new PokemonModel
        {
            Name = "myPokemon",
            Description = "Desc",
            Habitat = "Habitat",
            IsLegendary = true
        };

        // Act & Assert
        ClassicAssert.AreEqual("myPokemon", sut.Name);
        ClassicAssert.AreEqual("Desc", sut.Description);
        ClassicAssert.AreEqual("Habitat", sut.Habitat);
        ClassicAssert.IsTrue(sut.IsLegendary);
    }
}