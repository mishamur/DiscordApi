using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApi.Models
{
    class PlayerStat
    { 
        public string SummonerName { get; set; }
        public int? Kills { get; set; }
        public int? Deaths { get; set; }
        public int? Assists { get; set; }
        public string ChampionName { get; set; }
        public int? ParticipantId { get; set; }
        public int? QuadraKills { get; set; }
        public int? PentaKills { get; set; }
        public int? TotalDamageDealtToChampions { get; set; }
        public int? Spell1Casts { get; set; }
        public int? Spell4Casts { get; set; }
        public int? VisionScore { get; set; }
        public bool Win { get; set;  }
        public int? TotalMinionsKilled { get; set;  }
        public int? NeutralMinionsKilled { get; set; }
        public int? TotalTimeSpentDead { get; set;  }
        public int? WardsPlaced { get; set; }
        public int? GoldEarned { get; set; }
        public int? TimeCCingOthers { get; set; }

        public StringBuilder GetStat()
        {
            StringBuilder stat = new StringBuilder();
            stat.AppendLine("Имя призывателя: " + SummonerName);
            stat.AppendLine("Отыграл на: " + ChampionName);
            stat.AppendLine($"КDA: {Kills} / {Deaths} / {Assists}");
            stat.AppendLine("Заработал голдишки: " + GoldEarned);
            stat.AppendLine("Нанёс урона вражинам: " + TotalDamageDealtToChampions);
            stat.AppendLine("Показатель контроля: " + TimeCCingOthers);
            stat.AppendLine("Скастил q-ку: " + Spell1Casts);
            stat.AppendLine("Скастил r-ку: " + Spell4Casts);
            stat.AppendLine("Показатель обзора " + VisionScore);
            stat.AppendLine("Поставил вардиков: " + WardsPlaced);
            stat.AppendLine("Нафармил крипчиков: " + (TotalMinionsKilled + NeutralMinionsKilled));
            stat.AppendLine("Чилил в таверне секунд: " + TotalTimeSpentDead);
            if (Win)
                stat.AppendLine("Изи победная");
            else
                stat.AppendLine("Опять прое*али");
            if (PentaKills > 0)
                stat.AppendLine("Поздравляем с первым и последним пентакиллом в твоей жизни");
            
            return stat;
        }
        public override string ToString()
        {
            return GetStat().ToString();
        }

    }
}
