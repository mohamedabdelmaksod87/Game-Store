namespace GameStore.Api.Dtos;

public record class GameDto(
  int Id,
  string Name,
  GenreDTO Genre,
  decimal Price,
  DateOnly ReleaseDate
);

