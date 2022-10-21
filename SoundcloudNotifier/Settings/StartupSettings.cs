namespace SoundcloudNotifier.Settings; 

public class StartupSettings {
  public char CommandPrefix { get; set; } = '.';
  public string DiscordToken { get; set; } = "your token here";
  
  public ulong? GuildId { get; set; }
  public ulong? ChannelId { get; set; }
}