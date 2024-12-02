using NUnit.Framework.Legacy;
using Pokedex.Models;

namespace PokedexUnitTests.Models;

[TestFixture]
public class FunTranslationSuccessFixture
{
    [Test]
    public void FunTranslationSuccess_DefaultValues_SetFields()
    {
        // Arrange
        var sut = new FunTranslationSuccess();

        // Act & Assert
        ClassicAssert.AreEqual(0, sut.Total);
    }

    [Test]
    public void FunTranslationSuccess_SetValues_AreAssignedCorrectly()
    {
        // Arrange
        var sut = new FunTranslationSuccess()
        {
            Total = 10
        };

        // Act & Assert
        ClassicAssert.AreEqual(10, sut.Total);

    }
}