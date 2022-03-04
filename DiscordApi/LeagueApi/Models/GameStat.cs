using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiscordApi.LeagueApi.Models
{
    [Owned]
    public class GameStat
    {
        public string? gameStartTimestamp { get; set; }
        public string? gameEndTimestamp { get; set; }
        public override string ToString()
        {
            DateTime timeStart = DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(gameStartTimestamp));
            TimeSpan tsStart = TimeSpan.Parse(timeStart.ToString("HH:mm:ss"));

            DateTime timeEnd = DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(gameEndTimestamp));
            TimeSpan tsEnd = TimeSpan.Parse(timeEnd.ToString("HH:mm:ss"));

            return $"gameStart: {timeStart.Date.ToLongDateString()} {tsStart}; gameEnd: {timeEnd.Date.ToLongDateString()} {tsEnd}";
        }

    }
}
