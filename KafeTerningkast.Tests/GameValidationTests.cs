using KafeTerningkast.Models;
using KafeTerningkast.Services;

namespace KafeTerningkast.Tests;

public class GameValidationTests
{
    private static BoardGame Valid() => new()
    {
        Name = "test",
        MinPlayers = 2,
        MaxPlayers = 4,
        PlayTimeMinutes = 30,
        Rating = 3,
        Category = "x",
        Description = "y"
    };

    [Fact]
    public void Validate_EmptyName_HasError()
    {
        var game = Valid();
        game.Name = "";
        var errors = GameValidation.Validate(game);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Validate_MaxLowerThanMin_HasError()
    {
        var game = Valid();
        game.MinPlayers = 4;
        game.MaxPlayers = 2;
        var errors = GameValidation.Validate(game);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Validate_ValidGame_NoErrors()
    {
        var errors = GameValidation.Validate(Valid());
        Assert.Empty(errors);
    }
}