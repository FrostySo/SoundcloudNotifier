using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace SoundcloudNotifier; 

public class CommandHandlingService {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services) {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync() {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // this class is where the magic starts, and takes actions upon receiving messages
        public async Task MessageReceivedAsync(SocketMessage rawMessage) {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message)) {
                return;
            }

            if (message.Source != MessageSource.User) {
                return;
            }
            var messageStr = message.ToString().ToLower();

            var argPos = 0;

            char prefix = Program.StartupSettings.CommandPrefix;

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos))) {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result) {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified) {
                System.Console.WriteLine($"Command failed to execute for {context.Channel.Name}!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess) {
                System.Console.WriteLine($"Command {Program.StartupSettings.CommandPrefix}{command.Value.Name} executed successfully");
                return;
            }


            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry, ... something went wrong -> {result.ErrorReason}!");
        }
    }