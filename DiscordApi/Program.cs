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
                Token = "OTIyMDMwMDE0MjAxOTg3MDgz.GP660R.NYhXaDtlrBZWHj6o3Vjdgq16ICJWFQTUXNSMJE",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });
            client.UseVoiceNext();

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
