using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DiscordApi.commands;
using DSharpPlus.VoiceNext;

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
                Token = "your_token",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });
            client.UseVoiceNext();

            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "//" }
            });

            commands.RegisterCommands<CommandModule>();
            
                
            await client.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
