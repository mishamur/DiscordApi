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
        public static bool RegisterUsers(string SummonerName, ulong AuthorId, ulong GuildId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                string puuid = LeagueStat.GetPuuidByName(SummonerName);
                if (puuid != null)
                {
                    User user = new User() { SummName = SummonerName, AuthorId = AuthorId, GuildId = GuildId, Puuid = puuid };
                    if (!db.Users.Contains(user))
                    {
                        bool flag = true;
                        foreach(var value in db.Users)
                        {
                            if(value.Puuid == user.Puuid || value.SummName.Equals(user.SummName) || value.AuthorId == user.AuthorId)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            db.Users.Add(user);
                            Console.WriteLine(user);
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
