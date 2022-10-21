# Soundcloud Tracker

A discord bot that tracks artists on soundcloud and notifies you when they release a new song

## Installation

**Important: Requires .NET 6**

Clone the repo

cd  into the root directory of the project and run the dotnet command publish as so, it defaults to the current os you're running it on

`dotnet publish -p:PublishSingleFile=true -c Release --self-contained true --runtime win-x64` Change runtime to whatever runtime you are using. List can be found [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

Then you should be able to just run the program natively and .NET should handle the rest. It would be located in `/bin/release/publish/platform`

## Usage
On Initial Run "Settings.json" will be created, fill it in with your discord token.

Current Commands are

```
help - Shows all commands
artistadd - Adds an Artist to track
artistremove - Removes an artist from the tracking db
indb - Gets all artists the bot is currently tracking from the database
changeprefix
admin commands - just a basic bunch of unnecessary commands (requires owner)
```

To view the full list of commands type [prefix]help where prefix is the prefix you chose. By default it is `.`

## Contributing
Pull requests are welcome. 