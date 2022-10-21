using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace SoundcloudNotifier.Services;

public class LoggingService {
  private readonly IServiceProvider _services;

  public LoggingService(IServiceProvider services) {
    _services = services;
  }
  
#pragma warning disable CS1998
  public async Task InitializeAsync() {
#pragma warning restore CS1998
    var client = _services.GetRequiredService<DiscordSocketClient>();
    var commands = _services.GetRequiredService<CommandService>();
    client.Log += LogAsync;
    commands.Log += LogAsync;
  }

  public void Log(LogMessage m) {
    LogAsync(m).GetAwaiter().GetResult();
  }

  public Task LogAsync(LogMessage m) {
    if (m.Exception is CommandException commandException) {
      Console.WriteLine($"[{m.Source}][Command/{m.Severity}] - {commandException.Command.Aliases.First()} failed to execute in {commandException.Context.Channel}:");
      Console.WriteLine(commandException);
    }else {
      Console.WriteLine($"[{m.Source}][General/{m.Severity}] - {m}");
    }

    return Task.CompletedTask;
  }
}