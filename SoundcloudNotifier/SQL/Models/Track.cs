using SQLite;

namespace SoundcloudNotifier.SQL.Models; 

public class Track {
  
  [NotNull]
  [PrimaryKey]
  public long TrackId { get; set; }
  
  [NotNull]
  public long ArtistId { get; set; }
}