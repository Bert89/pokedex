using NUnit.Framework.Legacy;
using Pokedex.Models;

namespace PokedexUnitTests.Models;

[TestFixture]
public class FunTranslationContentsFixture
{
    [Test]
    public void FunTranslationContents_DefaultValues_SetFields()
    {
        // Arrange
        var sut = new FunTranslationContents();

        // Act & Assert
        ClassicAssert.IsNull(sut.Text);
        ClassicAssert.IsNull(sut.Translated);
        ClassicAssert.IsNull(sut.Translation);
    }

    [Test]
    public void FunTranslationContents_SetValues_AreAssignedCorrectly()
    {
        // Arrange
        var sut = new FunTranslationContents
        {
            Text = "T1",
            Translated = "T2",
            Translation = "T3"
        };

        // Act & Assert
        ClassicAssert.AreEqual("T1", sut.Text);
        ClassicAssert.AreEqual("T2", sut.Translated);
        ClassicAssert.AreEqual("T3", sut.Translation);

    }
}