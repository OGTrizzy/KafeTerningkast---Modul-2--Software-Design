using System.ComponentModel.DataAnnotations;

namespace KafeTerningkast.Models;

public class BoardGameForm
{
    [Required(ErrorMessage = "navn er påkrevd")]
    public string Name { get; set; } = "";

    [Range(1, 99, ErrorMessage = "min spillere må være 1")]
    public int MinPlayers { get; set; } = 1;
    [Range(1, 99, ErrorMessage = "max spillere må være lik eller høyere enn min spillere")]
    public int MaxPlayers { get; set; } = 4;

    [Range(1, 1000, ErrorMessage = "spilletid må være over 0")]
    public int PlayTimeMinutes { get; set; } = 30;

    public string Category { get; set; } = "";

    [Range(1, 6, ErrorMessage = "terningkast må være mellom 1 - 6")]
    public int Rating { get; set; } = 3;

    public string Description { get; set; } = "";

    public BoardGame ToGame() => new()
    {
        Name = Name.Trim(),
        MinPlayers = MinPlayers,
        MaxPlayers = MaxPlayers,
        PlayTimeMinutes = PlayTimeMinutes,
        Category = Category,
        Rating = Rating,
        Description = Description
    };

    public static BoardGameForm FromGame(BoardGame g) => new()
    {
        Name = g.Name,
        MinPlayers = g.MinPlayers,
        MaxPlayers = g.MaxPlayers,
        PlayTimeMinutes = g.PlayTimeMinutes,
        Category = g.Category,
        Rating = g.Rating,
        Description = g.Description
    };
}
