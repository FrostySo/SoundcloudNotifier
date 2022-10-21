using SQLite;

namespace SoundcloudNotifier.SQL.Models; 

public class Artist {

  [PrimaryKey]
  public long Id { get; set; }
  
  [NotNull]
  public string Name { get; set; } = null!;

  [NotNull]
  public string Username { get; set; } = null!;
}