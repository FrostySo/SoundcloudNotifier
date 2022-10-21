using Discord;
using Discord.Commands;
using SoundcloudNotifier.Extensions;

namespace SoundcloudNotifier.Commands; 

[Name("Help Commands")]
[Remarks("📚")]
public class HelpCommand : ModuleBase {
  private readonly CommandService _commandService;
  public HelpCommand(CommandService commandService) {
    this._commandService = commandService;
  }

  [Command("help"), Alias("assist"), Summary("Shows help menu.")]
  public async Task Help([Remainder] string? command = null) {
    var userInfo = Context.User as IGuildUser;
    bool isOwner = false;
    if (userInfo != null ) {
        isOwner = userInfo.Guild.OwnerId == userInfo.Id;
    }else{
      if (Program.StartupSettings.GuildId.HasValue) {
        var guild = await Context.Client.GetGuildAsync(Program.StartupSettings.GuildId.Value);
        if(guild != null)
          isOwner = guild.OwnerId == Context.User.Id;
      }
    }
    var helpEmbed = _commandService.GetDefaultHelpEmbed(command, Program.StartupSettings.CommandPrefix.ToString(),isOwner);
    await Context.Channel.SendMessageAsync(embed: helpEmbed);
  }
}