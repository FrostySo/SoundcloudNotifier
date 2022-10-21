using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SoundcloudNotifier.DiscordActions;
using SoundcloudNotifier.Model;
using SoundcloudNotifier.SQL;
using SoundcloudNotifier.SQL.Models;
using SoundcloudNotifier.Util;

namespace SoundcloudNotifier.Services; 

public class SoundcloudService {

  private List<Artist> _artists;
  private readonly Stopwatch _refreshArtistsStopwatch;
  private readonly SoundcloudTrackingDb _db;
  private CancellationTokenSource _cancellationToken = null!;
  private readonly LoggingService _loggingService;
  private readonly DiscordSocketClient _discordClient;
  public SoundcloudService(IServiceProvider? serviceProvider) {
    if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
    this._loggingService = serviceProvider.GetRequiredService<LoggingService>()!;
    this._discordClient = serviceProvider.GetRequiredService<DiscordSocketClient>()!;
    _db = new SoundcloudTrackingDb();
    _db.CreateIfNotExists();
    _refreshArtistsStopwatch = Stopwatch.StartNew();
    _artists = _db.GetAllArtists();
  }

  public void Start() {
    _cancellationToken = new CancellationTokenSource();
    var thread = new Thread(async () => {
      while (!_cancellationToken.IsCancellationRequested) {
        try {
          await LoopAsync();
          await Task.Delay(1000);
        }catch (Exception e) {
          await _loggingService.LogAsync(new LogMessage(LogSeverity.Error, "Soundcloud Service", null, e));
        }
      }
    }) {
      IsBackground = true
    };
    thread.Start();
  }

  public void Stop() {
    _cancellationToken.Cancel();
  }

  private int _statusMessageIndex = 0;
  private async Task RotateStatusAsync() {
    string[] status = {$"Tracking {_artists.Count} Artists",$"Last checked {_totalArtistsChecked}/{_artists.Count} Artists {DateTime.Now.Subtract(_lastUpdated).TotalSeconds} seconds ago"};
    await _discordClient.SetActivityAsync(new Game(status[_statusMessageIndex++ % status.Length]));
  }
 
  private bool WasReleasedToday(DateTime datePublished) {
    var currentTime = DateTime.Now;
    return (datePublished.Day == currentTime.Day && datePublished.Month == currentTime.Month &&
        datePublished.Year == currentTime.Year);
  }

  private int _totalArtistsChecked = 0;
  private DateTime _lastUpdated = DateTime.Now;
  private async Task LoopAsync() {
    
    if(_refreshArtistsStopwatch.Elapsed.TotalMinutes >=1) {
      _artists = _db.GetAllArtists();
      _refreshArtistsStopwatch.Restart();
      await RotateStatusAsync();
    }
    
    int artistsChecked = 0;
    for (var i = 0; i < _artists.Count; i++) {
      var artist = _artists[i];
      var tracks = await SoundCloudUtil.GetUserTracksAsync(artist.Id, 20);//api doesn't work well unless limit is 20 always
      if(tracks is null) continue;
      ++artistsChecked;
      using var conn = _db.SimpleDbConnection();
      foreach (var collection in tracks.Collection) {
        if (!_db.TrackInList(conn, collection.Id)) {
          if (WasReleasedToday(collection.DisplayDate.ToLocalTime())) {
              Program.MessageQueue.Enqueue(new DiscordSongNotificationAction(artist,collection));
          }
          _db.AddTrack(conn,collection); 
        }
      }
      await Task.Delay(1000 * 25);
    }
    _lastUpdated = DateTime.Now;
    _totalArtistsChecked = artistsChecked;
  }
}