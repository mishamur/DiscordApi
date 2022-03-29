using DiscordApi.LeagueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MP3Sharp;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern = @"-\w+";
            string content = "gdsffsd -asd -ss";
            Regex regex = new Regex(pattern);
            

            foreach(Match m in regex.Matches(content))
            {
                Console.WriteLine(m.Value);
            }


            
        }


    }
}
