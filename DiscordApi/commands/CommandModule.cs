using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DiscordApi.LeagueApi;
using DiscordApi.Register;

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

        [Command("lStat")]
        public async Task PlayStatus(CommandContext context)
        {
            try
            {
                StringBuilder asnwer = await LeagueStat.MainAsync(context.Message.Author.Id);
                Console.WriteLine(asnwer);
                await context.RespondAsync(asnwer.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Command("test")]
        public async Task Test(CommandContext context, string content)
        {
            await context.RespondAsync($"content: {content}, AuthorId: {context.Message.Author.Id}," +
                 $" GuildId: {context.Guild.Id}");
        } 

        [Command("register")]
        public async Task Register(CommandContext context, string content)
        {
            if(content != "" && content != null)
            {
               
                bool result = RegisterUser.RegisterUsers(content, context.Message.Author.Id, context.Guild.Id);
                Console.WriteLine(result);
                if (result)
                {
                    await context.RespondAsync("successfull");
                    return;
                }
                
            }
            await context.RespondAsync("Некорректный ввод");
   
        }
    }
}
