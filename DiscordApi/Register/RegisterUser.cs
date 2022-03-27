using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscordApi.LeagueApi;
using DiscordApi.LeagueApi.Models;

namespace DiscordApi.Register
{
    public static class RegisterUser
    {
        public static bool RegisterUsers(string summonerName, ulong authorId, ulong guildId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                string puuid = LeagueStat.GetPuuidByName(summonerName);
                if (puuid != null)
                {
                    
                    if (db.Users.FirstOrDefault(x => x.Puuid == puuid) == null)
                    {

                        User user = new User() { SummName = summonerName, AuthorId = authorId, GuildId = guildId, Puuid = puuid };
                        db.Users.Add(user);
                        Console.WriteLine(user);
                        db.SaveChanges();
                        Console.WriteLine("всё сохранено");
                        return true;       
                    }
                    else
                    {
                        User user = db.Users.FirstOrDefault(x => x.Puuid == puuid);
                        if(!user.SummName.Equals(summonerName) || user.AuthorId == null || user.GuildId == null)
                        {
                            user.AuthorId = authorId;
                            user.GuildId = guildId;
                            user.SummName = summonerName;
                            db.Users.Update(user);
                            db.SaveChanges();
                            Console.WriteLine("всё сохранено");
                            return true;
                        }
                    }
                }
                Console.WriteLine("ошибка добавления пользователя");
                return false;
            }
        }


    }
}
