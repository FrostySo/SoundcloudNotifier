using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundcloudNotifier;
using SoundcloudNotifier.DiscordActions;
using SoundcloudNotifier.Services;
using SoundcloudNotifier.Settings;
using SoundcloudNotifier.Util;

LoadSettings();
SoundCloudUtil.LoadClientIdFromCache();
_serviceProvider = ConfigureServices().BuildServiceProvider();
var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
Thread queueThread = new Thread(() => CheckQueueThread(client)) {
  IsBackground = true
};

queueThread.Start();
#pragma warning disable CS1998
client.ChannelDestroyed += async delegate(SocketChannel channel) {
#pragma warning restore CS1998
  if (channel.Id == StartupSettings.ChannelId) {
    StartupSettings.ChannelId = null;
  }
};

#pragma warning disable CS1998
client.LeftGuild += async delegate(SocketGuild guild) {
  if (guild.Id == StartupSettings.GuildId) {
    StartupSettings.GuildId = null;
  }
};
await _serviceProvider.GetRequiredService<LoggingService>().InitializeAsync();     
await _serviceProvider.GetRequiredService<CommandHandlingService>().InitializeAsync();
var soundcloudService = new SoundcloudService(_serviceProvider);
soundcloudService.Start();
var token = StartupSettings.DiscordToken;
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

Thread.Sleep(-1);

public partial class Program {
  public const string SaveFile = "Settings.json";
  public static StartupSettings StartupSettings { get; protected set; } = null!;

  public static Queue<IDiscordAction> MessageQueue = new();
  private static IServiceProvider _serviceProvider = null!;
  static async void CheckQueueThread(DiscordSocketClient socketClient) {
    bool notifiedNoValue = false;
    var loggingService = _serviceProvider.GetRequiredService<LoggingService>();
    while (true) {
      if (StartupSettings.GuildId.HasValue && StartupSettings.ChannelId.HasValue) {
        if (MessageQueue.Count > 0) {
          try {
            var msg = MessageQueue.Peek();
            if (await msg.SendActionAsync(socketClient)
                  .ConfigureAwait(true)) {
              MessageQueue.Dequeue();
            }
          }catch (Exception e) {
            await loggingService.LogAsync(new LogMessage(LogSeverity.Error,"Queue Thread",null,e));
          }
        }
        notifiedNoValue = false;
      }else {
        if (!notifiedNoValue) {
          await loggingService.LogAsync(new LogMessage(LogSeverity.Warning, "Queue Thread",
            "Startup Settings is missing either guild id, channel id or both. Discord notifications will be disabled until it is filled in"));
           notifiedNoValue = true;
        } 
      }
      Thread.Sleep(1000);
    }
  }
  
  static IServiceCollection ConfigureServices() {
    return new ServiceCollection()
      .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig() {
        GatewayIntents = GatewayIntents.All, //You can change this to intents you're only wanting to use
        LogGatewayIntentWarnings = false,
      }))
      .AddSingleton<CommandService>()
      .AddSingleton<CommandHandlingService>()
      .AddSingleton<LoggingService>();
  }

  static void Save(StartupSettings startupSettings) {
    File.WriteAllText(SaveFile,JsonConvert.SerializeObject(new StartupSettings(),Formatting.Indented));
  }

  static void PromptMessageThenExit(string message) {
    Console.WriteLine(message);
    Console.WriteLine("Press any key to exit");
    Console.ReadKey();
    Environment.Exit(1);
  }

 public static void LoadSettings() {
    if (!File.Exists(SaveFile)) {
        Save(new StartupSettings());
        PromptMessageThenExit($"Please fill in the details in located in {SaveFile}");
    }
    
    StartupSettings = JsonConvert.DeserializeObject<StartupSettings>(File.ReadAllText(SaveFile))!;
    if (String.IsNullOrEmpty(StartupSettings!.DiscordToken)) {
        PromptMessageThenExit($"Please enter a discord token in {SaveFile}");
    }
 }

  public static void SaveSettings() {
    Save(StartupSettings);
  }
}
