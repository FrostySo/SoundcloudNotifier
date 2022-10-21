using SoundcloudNotifier.Model;
using SoundcloudNotifier.SQL.Models;
using SQLite;

namespace SoundcloudNotifier.SQL; 

public class SoundcloudTrackingDb : DbHandler {
  
  private const string dbName = "soundcloud.db";
  public SoundcloudTrackingDb() : base(dbName) { }

  protected override void CreateTables(SQLiteConnection conn) {
    conn.CreateTable<Artist>();
    conn.CreateTable<Track>();
  }

  public bool AddTrack(SQLiteConnection connection,Collection collection) {
    lock (Padlock) {
      return connection.Insert(new Track() {
        ArtistId = collection.UserId,
        TrackId = collection.Id,
      }) > 0;
    }
  }

  public bool AddArtist(SQLiteConnection connection,Artist artist) {
    lock (Padlock) {
      return connection.Insert(artist) > 0;
    }
  }
  
  public bool AddArtist(Artist artist) {
    lock (Padlock) {
      using (var db = SimpleDbConnection()) {
        return db.Insert(artist) > 0;
      }
    }
  }

  public bool AddArtist(string name, long Id, string username) {
    using (var conn = SimpleDbConnection()) {
      return AddArtist(conn, new Artist() {
        Name = name,
        Id = Id, 
        Username = username
      });
    }
  }

  public bool RemoveArtist(string artistName) {
    using var conn = SimpleDbConnection();
    lock (Padlock) {
      return conn.ExecuteScalar<int>("DELETE FROM Artist WHERE Username LIKE ?", $"%{artistName}%") > 0;
    }
  }
  
  public List<Track> GetAllArtists(SQLiteConnection connection) {
    lock (Padlock) {
      return connection.Query<Track>("SELECT * FROM Artist");
    }
  }
  
  public List<Artist> GetAllArtists() {
    lock (Padlock) {
      using var conn = SimpleDbConnection();
      return conn.Query<Artist>("SELECT * FROM Artist");
    }
  }


  public List<Track> GetAllTracks(SQLiteConnection connection,long artistId) {
    lock (Padlock) {
      return connection.Query<Track>("SELECT * FROM Artist WHERE Id = {atristId}");
    }
  }

  public bool TrackInList(SQLiteConnection conn, long trackId) {
    lock (Padlock) {
      return ColumnExists(conn, "Track", "TrackId",trackId.ToString());
    }
  }
  
  public bool ArtistInList(string username) {
    lock (Padlock) {
      using (var conn = SimpleDbConnection()) {
        return ColumnContains(conn, "Artist", "Username",username);
      }
    }
  }
  
  public bool ArtistInList(SQLiteConnection conn, string username) {
    lock (Padlock) {
      return ColumnContains(conn, "Artist", "Username",username);
    }
  }

}