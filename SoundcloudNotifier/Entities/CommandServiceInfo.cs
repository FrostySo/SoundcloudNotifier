using Discord.Commands;

namespace SoundcloudNotifier.Entities; 

public record CommandServiceInfo(CommandInfo CommandInformation, ModuleInfo ModuleInformation,
    string CommandParameters, string CommandAliases);

