using KafeTerningkast.Models;
namespace KafeTerningkast.Services;

public static class GameValidation
{
    public static List<string> Validate(BoardGame game)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(game.Name))
            errors.Add("navn mangler");
        if (game.MinPlayers < 1)
            errors.Add("MinPlayers må være minst 1");
        if (game.MaxPlayers < game.MinPlayers)
            errors.Add("MaxPlayers kan ikke være lavere enn MinPlayers");
        if (game.PlayTimeMinutes <= 0)
            errors.Add("PlayTimeMinutes må være større enn 0");
        if (game.Rating < 1 || game.Rating > 6)
            errors.Add("ratingen må være innenfor 1 - 6");

        return errors;
    }
}
