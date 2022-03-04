using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DiscordApi.LeagueApi.Models
{
    public class User
    {
        [Key]
        public string Puuid { get; set; }
        public ulong? AuthorId { get; set; }
        public ulong? GuildId { get; set; }
        public string? SummName { get; set; }
        public List<PlayerGameStat> PlayerGameStats { get; set; } = new();
        public override string ToString()
        {
            return Puuid;
        }

    }
}
