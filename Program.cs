using KafeTerningkast.Components;
using KafeTerningkast.Models;
using KafeTerningkast.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//scoped = one instance per user session, singleton = one instance for all users
builder.Services.AddSingleton<GameCatalog>();
builder.Services.AddScoped<GameNightState>();

builder.Services.AddHttpClient<GameApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

var gamesApi = app.MapGroup("/api/games");
gamesApi.MapGet("/", (GameCatalog catalog) =>
    Results.Ok(catalog.GetAll()));

gamesApi.MapGet("/{id:int}", (int id, GameCatalog catalog) =>
{
    var game = catalog.GetById(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

gamesApi.MapPost("/", (BoardGame game, GameCatalog catalog) =>
{
    var errors = GameValidation.Validate(game);
    if (errors.Count > 0)
        return Results.BadRequest(new { errors });

    var created = catalog.Add(game);
    return Results.Created($"/api/games/{created.Id}", created);
});

gamesApi.MapPut("/{id:int}", (int id, BoardGame game, GameCatalog catalog) =>
{
    var errors = GameValidation.Validate(game);
    if (errors.Count > 0)
        return Results.BadRequest(new { errors });

    if (!catalog.Update(id, game))
        return Results.NotFound();

    return Results.NoContent();
});

gamesApi.MapDelete("/{id:int}", (int id, GameCatalog catalog) =>
{
    if (!catalog.Delete(id))
        return Results.NotFound();

    return Results.NoContent();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
