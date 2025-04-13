namespace GameStore.Api.Entities;

public class Genre
{
  public int Id { get; private init; }

  public required string Name { get; set; }

  public ICollection<Game> Games { get; } = [];
}
