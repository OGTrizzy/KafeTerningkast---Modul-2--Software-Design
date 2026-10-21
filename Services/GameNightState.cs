using KafeTerningkast.Models;

namespace KafeTerningkast.Services;

public class GameNightState
{
    private readonly List<BoardGame> _selected = [];
    public event Action? Changed;
    public IReadOnlyList<BoardGame> SelectedGames => _selected;
    public int Count => _selected.Count;
    public int TotalPlayTimeMinutes => _selected.Sum(g => g.PlayTimeMinutes);
    public bool Contains(int gameId) => _selected.Any(g => g.Id == gameId);

    public bool Add(BoardGame game)
    {
        if (Contains(game.Id))
            return false;

        _selected.Add(game);
        Notify();
        return true;
    }

    public bool Remove(int gameId)
    {
        var removed = _selected.RemoveAll(g => g.Id == gameId) > 0;
        if (removed)
            Notify();
        return removed;
    }

    private void Notify() => Changed?.Invoke();
}
