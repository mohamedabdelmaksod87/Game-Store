namespace GameStore.Api.Entities;

public class Game
{
  public int Id { get; private init; }

  public required string Name { get; set; }

  public required decimal Price { get; set; }

  public required DateOnly ReleaseDate { get; set; }

  public required int GenreId { get; set; }

  public Genre Genre { get; set; } = null!;
}
