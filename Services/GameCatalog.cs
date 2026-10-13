using KafeTerningkast.Models;

namespace KafeTerningkast.Services;

public class GameCatalog
{
    private readonly List<BoardGame> _games =
    [
        new()
        {
            Id = 1,
            Name = "Catan",
            MinPlayers = 3,
            MaxPlayers = 4,
            PlayTimeMinutes = 90,
            Category = "Strategi",
            Rating = 5,
            Description = "Bygg veier, byer og bosetninger på øya Catan. Samle ressurser og handel med andre spillere."
        },
        new()
        {
            Id = 2,
            Name = "Ticket to Ride",
            MinPlayers = 2,
            MaxPlayers = 5,
            PlayTimeMinutes = 60,
            Category = "Familie",
            Rating = 4,
            Description = "Bygg jernbaner over hele Nord-Amerika. Samle kort og fullfør ruter for å score poeng."
        },
        new()
        {
            Id = 3,
            Name = "Pandemic",
            MinPlayers = 2,
            MaxPlayers = 4,
            PlayTimeMinutes = 45,
            Category = "Samarbeid",
            Rating = 5,
            Description = "Samarbeid med andre spillere for å stoppe globale sykdomsutbrudd. Bruk strategiske ferdigheter for å redde verden."
        },
        new()
        {
            Id = 4,
            Name = "Carcassonne",
            MinPlayers = 2,
            MaxPlayers = 5,
            PlayTimeMinutes = 35,
            Category = "Familie",
            Rating = 4,
            Description = "Bygg landskapet rundt middelalderbyen Carcassonne. Plasser brikker og følg med på poengsummen."
        },
        new()
        {
            Id = 5,
            Name = "7 Wonders",
            MinPlayers = 2,
            MaxPlayers = 7,
            PlayTimeMinutes = 30,
            Category = "Strategi",
            Rating = 5,
            Description = "Bygg en sivilisasjon gjennom tre tidsaldre. Samle ressurser, utvikle teknologi og konkurrer med andre spillere."
        },
        new()
        {
            Id = 6,
            Name = "Azul",
            MinPlayers = 2,
            MaxPlayers = 4,
            PlayTimeMinutes = 30,
            Category = "Familie",
            Rating = 4,
            Description = "Plasser fliser på brettet for å skape vakre mønstre. Samle poeng basert på flisplassering og strategiske valg."
        },
        new()
        {
            Id = 7,
            Name = "Terraforming Mars",
            MinPlayers = 1,
            MaxPlayers = 5,
            PlayTimeMinutes = 120,
            Category = "Strategi",
            Rating = 5,
            Description = "Terraform Mars ved å bygge byer, skaffe ressurser og utvikle teknologi. Konkurrer med andre selskaper for å bli den mest suksessrike."
        },
        new()
        {
            Id = 8,
            Name = "Dixit",
            MinPlayers = 3,
            MaxPlayers = 6,
            PlayTimeMinutes = 30,
            Category = "Familie",
            Rating = 4,
            Description = "Bruk fantasien og fortell historier med vakre kort. Gjett hvilke kort som matcher historien for å score poeng."
        }
    ];

    public IReadOnlyList<BoardGame> GetAll() => _games;
}
