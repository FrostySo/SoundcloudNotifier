using System.ComponentModel;
using Discord.Commands;

namespace SoundcloudNotifier.Commands; 

[Name("Admin Commands")]
[Remarks("👑")]
public class AdminCommands : ModuleBase{
   	
  [Discord.Commands.RequireOwner()]
  [Command("changeprefix"),Description("Changes your current command prefix to a new one")]
  public async Task ChangePrefix(string? newPrefix = null) {
	  
	  if (newPrefix?.Length != 1) {
		  await ReplyAsync("Prefix must be 1 character long");
		  return;
	  }

	  Program.StartupSettings.CommandPrefix = Convert.ToChar(newPrefix);
	  await ReplyAsync($"New Prefix is now `{newPrefix}`");
	  Program.SaveSettings();
  }
  
  [Discord.Commands.RequireOwner()]
  [Command("changechannelid"),Description("Changes your current channel id to a new one")]
  public async Task ChangeChannelId(string? newPrefix = null) {
	  if (ulong.TryParse(newPrefix, out var id)) {
		  if (Context.Guild != null) {
			  var guild = await Context.Client.GetGuildAsync(Program.StartupSettings.GuildId!.Value);
			  if (guild is null) {
				  await ReplyAsync("No guild is set. Please set one first before using this command");
				  return;
			  }
		  }
		  var temp = Program.StartupSettings.ChannelId;
		  Program.StartupSettings.GuildId = id;
		  Program.SaveSettings();
		  await ReplyAsync($"Changed Channel id from {temp} to {id}");
	  }else {
		  await ReplyAsync($"Invalid format.");
	  }
  }
  
  [Discord.Commands.RequireOwner()]
  [Command("changeguildid"),Description("Changes your current guild id to a new one")]
  public async Task ChangeGuildId(string? newPrefix = null) {
	  if (ulong.TryParse(newPrefix, out var id)) {
		  var guild = await Context.Client.GetGuildAsync(id);
		  if (guild is null) {
			  await ReplyAsync($"Guild is null. Bot does not have permission to view guild with id {id}");
			  return;
		  }

		  var temp = Program.StartupSettings.GuildId;
		  Program.StartupSettings.GuildId = id;
		  Program.SaveSettings();
		  await ReplyAsync($"Changed Guild id from {temp} to {id}");
	  }else {
		  await ReplyAsync($"Invalid format.");
	  }
  }
	
}