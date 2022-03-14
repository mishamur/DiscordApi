using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DiscordApi.LeagueApi;
using DiscordApi.Register;
using DSharpPlus.Entities;
using DSharpPlus;
using System.Drawing;
using System.IO;
using DiscordApi.LeagueApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.commands
{
    
    public class CommandModule : BaseCommandModule 
    {
        [Command("hello")]
        public async Task HelloCommand(CommandContext context)
        {
            List<string> helloRespose = new List<string>(new string[] {
            "Салют",
            "Оу, давно не виделись",
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

        [Command("getwr")]
        public async Task GetWinrate(CommandContext context, string summonerName)
        {
            string pathToImage = LeagueStat.GetWintate(summonerName).Result;

            DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder();
            messageBuilder.WithFile(pathToImage, new StreamReader(pathToImage).BaseStream);
            messageBuilder.Content = $"Винрейт игрока: {summonerName}";
            await context.RespondAsync(messageBuilder);
        }

        [Command("sendPicture")]
        public async Task SendPicture(CommandContext context)
        {
            List<PlayerGameStat> pgs = null;
            using(ApplicationContext db = new ApplicationContext())
            {
                pgs = db.PlayerGameStats.Include(p => p.User).Where(p => p.User.SummName == "W3akLink").ToList();
            }
            if(pgs != null)
            {
                Console.WriteLine("ща будет картинка");
                string path = Drawing.DrawingChart.DrawAndSave(pgs);
                DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder();
                //messageBuilder.WithFile(@"E:\pict\lel.jpg", new StreamReader(@"E:\pict\lel.jpg").BaseStream);
                messageBuilder.WithFile(path, new StreamReader(path).BaseStream);
                await context.RespondAsync(messageBuilder);
            }
            else
            {
                Console.WriteLine("pgs = null");
                await context.RespondAsync("err");
            }
            
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
