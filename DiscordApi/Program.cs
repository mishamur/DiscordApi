﻿using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DiscordApi.commands;

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
                Token = "OTIyMDMwMDE0MjAxOTg3MDgz.Yb7hXQ._48OK81p_Mqu5Hk72_h23wnDuag",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

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