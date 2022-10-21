using System.Globalization;
using Discord;
using Discord.WebSocket;
using SoundcloudNotifier.Model;
using SoundcloudNotifier.SQL.Models;

namespace SoundcloudNotifier.DiscordActions; 

public class DiscordSongNotificationAction : IDiscordAction {
  private readonly Artist _artist;
  private readonly Collection _collection;
  public DiscordSongNotificationAction(Artist artist,Collection collection) {
    this._artist = artist;
    this._collection = collection;
  }
  
  public async Task<bool> SendActionAsync(DiscordSocketClient discordClient) {
    var settings = Program.StartupSettings;
    var guild = discordClient.GetGuild(settings.GuildId!.Value);
    if (guild is null) return false;
    var messageChannel = guild.GetChannel(settings.ChannelId!.Value) as IMessageChannel;
    if (messageChannel is null) {
      return false;
    }

    var embedBuilder = new EmbedBuilder()
      .WithAuthor(_artist.Name)
      .WithTitle(GetSongTitleWithoutName(_collection.Title))
      .WithUrl(_collection.PermalinkUrl)
      .WithThumbnailUrl(_collection.ArtworkUrl)
      .WithColor(new Color(0,152,217))
      .WithFooter($"{_artist.Username}", _collection.User?.AvatarUrl ?? "https://i1.sndcdn.com/avatars-wQ2we7uDPoXzUVzW-qdr1Yg-t500x500.jpg")
      //.WithFooter($"{_artist.Username}", "https://i1.sndcdn.com/avatars-wQ2we7uDPoXzUVzW-qdr1Yg-t500x500.jpg")
      .WithCurrentTimestamp();

    if (!String.IsNullOrEmpty(_collection.Genre)) {
      embedBuilder
        .AddField("Genre",_collection.Genre,false);
    }
   
    var messageId =  await messageChannel.SendMessageAsync($"New Song From {_artist.Name}",false,embedBuilder.Build(),new RequestOptions() {
      RetryMode = RetryMode.AlwaysRetry
    });
    return messageId != null;
  }

  string GetSongTitleWithoutName(string? title) {
    if (title == null) return "unknown";
    var index = title.IndexOf("-")+1;
    return title.Substring(index, title.Length - index).TrimStart();
  }
}