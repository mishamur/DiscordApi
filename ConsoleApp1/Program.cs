using DiscordApi.LeagueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = (double)((int)((double)256 / 501 * 100 * 100 ))/ 100;
            Console.WriteLine(a);
           

            using(ApplicationContext db = new ApplicationContext())
            {
                
                PlayerGameStat playerStat12 = db.PlayerGameStats.FirstOrDefault(x => (x.MatchId == "RU_370052055" &&
                x.UserPuuid == "AVaUX1Fvu4NHC8Vm5-V8OdnbsS_4nLes-C1OqBrWVr611YSmd72ph_1_XrZCH2jiQ-fXRpshrgJgew"));

                if (playerStat12 == null) 
                {
                    Console.WriteLine("игра не найдена");
                }
                else
                {
                    Console.WriteLine("игра найдена");
                    Console.WriteLine(playerStat12.ToString());
                }
            }

            Console.WriteLine("complete");


        }
    }
}
