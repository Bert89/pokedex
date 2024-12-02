using NUnit.Framework.Legacy;
using Pokedex.Models;

namespace PokedexUnitTests.Models;

[TestFixture]
public class ErrorResponseFixture
{
    [Test]
    public void ErrorResponse_DefaultValues_SetFields()
    {
        // Arrange
        var sut = new ErrorResponse();

        // Act & Assert
        ClassicAssert.IsNull(sut.Details);
        ClassicAssert.IsNull(sut.Message);
    }

    [Test]
    public void ErrorResponse_SetValues_AreAssignedCorrectly()
    {
        // Arrange
        var sut = new ErrorResponse
        {
            Details = "Det",
            Message = "MyMessage",
        };

        // Act & Assert
        ClassicAssert.AreEqual("Det", sut.Details);
        ClassicAssert.AreEqual("MyMessage", sut.Message);


    }
}