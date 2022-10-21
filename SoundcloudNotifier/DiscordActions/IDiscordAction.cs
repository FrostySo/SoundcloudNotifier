using Discord.WebSocket;

namespace SoundcloudNotifier.DiscordActions; 

public interface IDiscordAction {
  Task<bool> SendActionAsync(DiscordSocketClient discordClient);
}