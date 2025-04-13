using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
  const string GetGameById = "GetGame";

  public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("games").WithParameterValidation();

    group.MapGet("/", async (GameStoreContext dbContext) =>
    {
      var games = await dbContext.Games
        .Include(g => g.Genre)
        .Select(g => g.ToDto())
        .AsNoTracking()
        .ToListAsync();

      return Results.Ok(games);
    });

    group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
    {
      var game = await dbContext.Games
        .AsNoTracking()
        .Include(g => g.Genre)
        .SingleOrDefaultAsync(g => g.Id == id);

      return game is null ? Results.NotFound() : Results.Ok(game.ToDto());
    }).WithName(GetGameById);

    group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
    {
      var genreExists = await dbContext.Genres.AnyAsync(g => g.Id == newGame.GenreId);
      if (!genreExists) return Results.NotFound($"Genre with id {newGame.GenreId} not found.");

      var game = newGame.CreateEntity();
      dbContext.Games.Add(game);
      await dbContext.SaveChangesAsync();

      return Results.CreatedAtRoute(GetGameById, new { id = game.Id });
    });

    group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
    {
      var existingGame = await dbContext.Games.FindAsync(id);
      if (existingGame is null) return Results.NotFound($"Game with id {id} not found.");

      var genreExists = await dbContext.Genres.AnyAsync(g => g.Id == updatedGame.GenreId);
      if (!genreExists) return Results.BadRequest($"Genre with id {updatedGame.GenreId} not found.");

      updatedGame.UpdateEntity(existingGame);

      await dbContext.SaveChangesAsync();
      return Results.NoContent();
    });

    group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
    {
      await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
      return Results.NoContent();
    });

    return group;
  }
}
