using System.Reflection;
using System.Text;
using DiscordApi.LeagueApi.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LeagueApi.Models
{
    [Owned]
    public class PlayerStat
    {
        [PlayerStatDescription("Имя призывателя")]
        public string SummonerName { get; set; }
        [PlayerStatDescription("Убийства")]
        public int? Kills { get; set; }
        [PlayerStatDescription("Смерти")]
        public int? Deaths { get; set; }
        [PlayerStatDescription("Помощь")]
        public int? Assists { get; set; }
        [PlayerStatDescription("Имя чемпиона")]
        public string ChampionName { get; set; }
        public int? ParticipantId { get; set; }
        [PlayerStatDescription("Кол-во квадрокиллов")]
        public int? QuadraKills { get; set; }
        [PlayerStatDescription("Пента")]
        public int? PentaKills { get; set; }
        [PlayerStatDescription("Урон по вражинам")]
        public int? TotalDamageDealtToChampions { get; set; }
        [PlayerStatDescription("Скастил Q-ку")]
        public int? Spell1Casts { get; set; }
        [PlayerStatDescription("Скастил R-ку")]
        public int? Spell4Casts { get; set; }
        [PlayerStatDescription("Показатель обзора")]
        public int? VisionScore { get; set; }
        [PlayerStatDescription("Победа?")]
        public bool Win { get; set;  }
        [PlayerStatDescription("Фармила крипов")]
        public int? TotalMinionsKilled { get; set;  }
        [PlayerStatDescription("Забрано крипов у лесника")]
        public int? NeutralMinionsKilled { get; set; }
        [PlayerStatDescription("Отдыхал на базе")]
        public int? TotalTimeSpentDead { get; set;  }
        [PlayerStatDescription("Зачем ты ставил столько вардов?")]
        public int? WardsPlaced { get; set; }
        [PlayerStatDescription("Кэш")]
        public int? GoldEarned { get; set; }
        [PlayerStatDescription("Кайтил времени")]
        public int? TimeCCingOthers { get; set; }

        public StringBuilder GetStat()
        {
            StringBuilder stat = new StringBuilder();
            
            foreach(var prop in this.GetType().GetProperties())
            {
                var descAttribute = prop.GetCustomAttribute<PlayerStatDescriptionAttribute>();
                if (descAttribute != null)
                    stat.AppendLine($"{descAttribute.Description} : {prop.GetValue(this)}");
            }
            
            return stat;
        }
        public override string ToString()
        {
            return GetStat().ToString();
        }

    }
}
