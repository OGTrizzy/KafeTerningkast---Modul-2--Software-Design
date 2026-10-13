namespace KafeTerningkast.Models;

public class BoardGame
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int PlayTimeMinutes { get; set; }
    public string Category { get; set; } = "";
    public int Rating { get; set; } //dice roll, 1-6
    public string Description { get; set; } = "";
}
