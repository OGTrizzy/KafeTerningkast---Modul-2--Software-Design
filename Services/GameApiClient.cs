using System.Net;
using System.Net.Http.Json;
using KafeTerningkast.Models;

namespace KafeTerningkast.Services;

public class GameApiClient
{
    private readonly HttpClient _http;

    public GameApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<BoardGame>> GetAllAsync()
    {
        var games = await _http.GetFromJsonAsync<List<BoardGame>>("api/games");
        return games ?? [];
    }

    public async Task<BoardGame?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"api/games/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BoardGame>();
    }

    public async Task<BoardGame?> CreateAsync(BoardGame game)
    {
        var response = await _http.PostAsJsonAsync("api/games", game);
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BoardGame>();
    }

    public async Task<bool> UpdateAsync(int id, BoardGame game)
    {
        var response = await _http.PutAsJsonAsync($"api/games/{id}", game);
        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.BadRequest)
            return false;

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/games/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        return response.IsSuccessStatusCode;
    }
}
