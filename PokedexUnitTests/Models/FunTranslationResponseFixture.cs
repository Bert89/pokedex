using NUnit.Framework.Legacy;
using Pokedex.Models;

namespace PokedexUnitTests.Models;

[TestFixture]
public class FunTranslationResponseFixture
{
    [Test]
    public void FunTranslationResponse_DefaultValues_SetFields()
    {
        // Arrange
        var sut = new FunTranslationResponse();

        // Act & Assert
        ClassicAssert.IsNull(sut.Contents);
        ClassicAssert.IsNull(sut.Success);
    }

    [Test]
    public void FunTranslationResponse_SetValues_AreAssignedCorrectly()
    {
        // Arrange
        var success = new FunTranslationSuccess();
        var contents = new FunTranslationContents();

        var sut = new FunTranslationResponse
        {
            Success = success,
            Contents = contents
        };

        // Act & Assert
        ClassicAssert.AreEqual(success, sut.Success);
        ClassicAssert.AreEqual(contents, sut.Contents);

    }
}