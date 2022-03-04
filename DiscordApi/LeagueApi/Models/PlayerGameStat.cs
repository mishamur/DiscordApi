using LeagueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DiscordApi.LeagueApi.Models
{
    public class PlayerGameStat
    {
        public string MatchId { get; set; } 
        public GameStat? GameStat { get; set; }
        public PlayerStat? PlayerStat { get; set; }
       
        public string UserPuuid { get; set; }
        public User User { get; set; }

        public override string ToString()
        {
            return MatchId + " " + PlayerStat?.ChampionName + " " + GameStat?.gameStartTimestamp;
        }

    }
}
