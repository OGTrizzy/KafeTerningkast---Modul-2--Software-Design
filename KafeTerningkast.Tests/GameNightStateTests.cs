using Xunit;
using KafeTerningkast.Models;
using KafeTerningkast.Services;

namespace KafeTerningkast.Tests;

public class GameNightStateTests
{
    private static BoardGame Sample(int id = 1, string name = "Catan", int minutes = 90) => new()
    {
        Id = id,
        Name = name,
        PlayTimeMinutes = minutes,
        MinPlayers = 2,
        MaxPlayers = 4,
        Category = "test",
        Rating = 4,
        Description = "x"
    };

    [Fact]
    public void Add_NewGame_IncreasesCount()
    {
        var state = new GameNightState();
        Assert.True(state.Add(Sample()));
        Assert.Equal(1, state.Count);
    }

    [Fact]
    public void Add_SameGameTwice_ReturnsFalse()
    {
        var state = new GameNightState();
        var game = Sample();
        state.Add(game);
        Assert.False(state.Add(game));
        Assert.Equal(1, state.Count);
    }

    [Fact]
    public void TotalPlayTime_SumsSelectedGames()
    {
        var state = new GameNightState();
        state.Add(Sample(1, "a", 30));
        state.Add(Sample(2, "b", 35));
        state.Add(Sample(3, "c", 15));
        Assert.Equal(80, state.TotalPlayTimeMinutes);
    }

    [Fact]
    public void Remove_LastGame_CountIsZero()
    {
        var state = new GameNightState();
        state.Add(Sample());
        state.Remove(1);
        Assert.Equal(0, state.Count);
        Assert.Equal(0, state.TotalPlayTimeMinutes);
    }
}
