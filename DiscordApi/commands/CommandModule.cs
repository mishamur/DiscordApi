using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordApi.commands
{
    
    public class CommandModule : BaseCommandModule 
    {
        

        [Command("hello")]
        public async Task HelloCommand(CommandContext context)
        {
            List<string> helloRespose = new List<string>(new string[] {
            "Салют",
            "Пошёл нахуй",
            "Оу, давно не виделись",
            "Чё нада",
            "Ну чё, поебёмся",
            "Привет",
            "Салам",
        });

            Random random = new Random();
            await context.RespondAsync($"{helloRespose.ElementAt(random.Next(0, helloRespose.Count - 1))}," +
                $" {context.Message.Author.Username}");
        }

        

    }
}
