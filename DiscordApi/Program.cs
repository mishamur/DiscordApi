using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DiscordApi.commands;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.DependencyInjection;
using DiscordApi.LeagueApi.Services;

namespace DiscordApi
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            DiscordClient client = new DiscordClient(new DiscordConfiguration()
            {
                Token = "yourtoken",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
            
            client.UseVoiceNext();

            var service = new ServiceCollection()
                .AddSingleton<PlayMusicService>()
                .AddSingleton<LeagueStatService>()
                .BuildServiceProvider();

            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });
            commands.RegisterCommands<CommandModule>();

            await client.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
