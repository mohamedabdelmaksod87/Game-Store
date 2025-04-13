using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GameMapping
{
  public static Game CreateEntity(this CreateGameDto game)
  {
    return new Game
    {
      Name = game.Name,
      GenreId = game.GenreId,
      Price = game.Price,
      ReleaseDate = game.ReleaseDate
    };
  }

  public static void UpdateEntity(this UpdateGameDto game, Game existingGame)
  {
    existingGame.Name = game.Name;
    existingGame.GenreId = game.GenreId;
    existingGame.Price = game.Price;
    existingGame.ReleaseDate = game.ReleaseDate;
  }

  public static GameDto ToDto(this Game game)
  {
    return new GameDto(
      game.Id,
      game.Name,
      new GenreDTO(game.Genre.Id, game.Genre.Name),
      game.Price,
      game.ReleaseDate
    );
  }
}
