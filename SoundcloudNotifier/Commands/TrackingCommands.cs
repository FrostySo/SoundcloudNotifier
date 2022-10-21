using System.Data;
using Discord.Commands;
using SoundcloudNotifier.SQL;
using SoundcloudNotifier.Util;

namespace SoundcloudNotifier.Commands; 

[Name("Tracking Commands")]
[Remarks("💻")]
public class TrackingCommands : ModuleBase {
  
  [Command("artistadd"), Alias("addartist"), Summary("Adds an Artist to track")]
  public async Task AddArtist(string? message = null) {
    if (String.IsNullOrEmpty(message)) {
      await ReplyAsync("Please enter something");
      return;
    }
    var dbHandler = new SoundcloudTrackingDb();
    if (dbHandler.ArtistInList(message)) {
      await ReplyAsync($"{message} Is already added for tracking");
      return;
    }

    var artist = await SoundCloudUtil.GetArtistAsync(message);
    if (artist != null) {
      if (dbHandler.AddArtist(artist)) {
        await ReplyAsync($"Found {artist.Name} with user id {artist.Id}, added to db");
      }else {
        await ReplyAsync($"Found {message} with user id {artist.Id}, but there was an error adding to db. Please Try Again.");
      }
    }else {
      await ReplyAsync($"{message} was not found on soundcloud");
    }
  }
  
  [Command("removeartist"), Alias("artistremove"), Summary("Removes an artist from the tracking db")]
  public async Task RemoveArtist(string? artistName = null) {
    if (String.IsNullOrEmpty(artistName)) {
      await ReplyAsync("Please enter something");
      return;
    }
    var dbHandler = new SoundcloudTrackingDb();
    if (dbHandler.RemoveArtist(artistName.Trim())) {
      await ReplyAsync($"Removed {artistName} from tracking. Changes will take at least a minute to update");
    }else {
      await ReplyAsync($"Could not find {artistName}. Ensure the username is correct");
    }
  }
  
  [Command("indb"), Alias("artists","inlist","tracking"), Summary("Gets all artists the bot is currently tracking from the database")]
  public async Task AllArtistsBeingTracked() {
    var dbHandler = new SoundcloudTrackingDb();
    var artistsList = dbHandler.GetAllArtists();
    if (artistsList.Count == 0) {
      await ReplyAsync("No artists being tracked atm.");
      return;
    }

    using var dataTable = new DataTable();
    dataTable.Columns.Add("Name");
    dataTable.Columns.Add("Username");
    artistsList.ForEach(artist => dataTable.Rows.Add(artist.Name,artist.Username));
    var formattedLines = FormatUtil.DiscordLineToFormattedMultiLine(AsciiTableGenerator.CreateAsciiTableFromDataTable(dataTable).ToString(), null);
    formattedLines[0] = formattedLines[0].Insert(0,$"Currently tracking {artistsList.Count} artists\n");
    foreach (var formattedLine in formattedLines) {
      await ReplyAsync(formattedLine);
    }
  }

  
  
}