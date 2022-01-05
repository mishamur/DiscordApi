using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using MingweiSamuel;
using MingweiSamuel.Camille.Enums;
using System.Text.RegularExpressions;
using LeagueApi.Models;
using System.Text;

namespace LeagueApi
{
    class Program
    {
        static void Main(string[] args)
        {

            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            const string api_key = "RGAPI-500b978f-cfb1-4536-85c4-0dcf8387c49a";

            var api = MingweiSamuel.Camille.RiotApi.NewInstance($"{api_key}");

            var nead = api.SummonerV4.GetBySummonerName(Region.RU, "neadecvatt228");
            Console.WriteLine(nead.Puuid);
            Console.WriteLine(nead.SummonerLevel);
            Console.WriteLine(nead.Name);
            Console.WriteLine("-------------------------");
            var ninj = api.SummonerV4.GetBySummonerName(Region.RU, "NiNjAZz");
            Console.WriteLine(ninj.Puuid);
            Console.WriteLine(ninj.SummonerLevel);
            Console.WriteLine(ninj.Name);
            Console.WriteLine("-------------------------");
            var weak = api.SummonerV4.GetBySummonerName(Region.RU, "W3akLink");
            Console.WriteLine(weak.Puuid);
            Console.WriteLine(weak.SummonerLevel);
            Console.WriteLine(weak.Name);
            Console.WriteLine("-------------------------");


            var matches = api.MatchV5.GetMatchIdsByPUUID(Region.Europe, nead.Puuid);

            matches.ToList().ForEach(x => Console.WriteLine(x));

            var match = api.MatchV5.GetMatch(Region.Europe, matches.First());
            Dictionary<string, object>.ValueCollection valueCollection = match._AdditionalProperties.Values;
            Dictionary<string, object>.KeyCollection keyCollection = match._AdditionalProperties.Keys;


            string last = valueCollection.ToList().Last().ToString();

            Console.WriteLine(match._AdditionalProperties.Values.ElementAt(0).GetType());


            JsonDocument json = JsonSerializer.Deserialize<JsonDocument>(last);

            JsonElement jsonGameVersion;

            bool isGameVersion = json.RootElement.TryGetProperty("gameVersion", out jsonGameVersion);

            if (isGameVersion)
                Console.WriteLine(jsonGameVersion.GetRawText());


            JsonElement jsonParticipants;
            bool isParticipants = json.RootElement.TryGetProperty("participants", out jsonParticipants);

            if (isParticipants)
            {
                //Console.WriteLine("jsonParticipants " + jsonParticipants.GetRawText());

                JsonElement.ArrayEnumerator particArray = jsonParticipants.EnumerateArray();

                particArray.ToList().ForEach(x => Console.WriteLine(x));
                Console.WriteLine("----------------------------------------------");

                
                PlayerStat neadec = null;
                PlayerStat ninjaz = null;
                PlayerStat weaklink = null;
                List<PlayerStat> playerStats = new List<PlayerStat>();
                foreach (var player in particArray)
                {
                    JsonElement jsonSummName;

                    if (player.TryGetProperty("summonerName", out jsonSummName))
                    {
                        string summonerName = jsonSummName.GetRawText();
                         
                        if (summonerName.Equals('"' + nead.Name + '"'))
                        {
                            neadec = GetPlayerStat(summonerName, player);
                            playerStats.Add(neadec);
                        }

                        if (summonerName.Equals('"' + ninj.Name + '"'))
                        {
                            ninjaz = GetPlayerStat(summonerName, player);
                            playerStats.Add(ninjaz);
                        }

                        if (summonerName.Equals('"' + weak.Name + '"'))
                        {
                            weaklink = GetPlayerStat(summonerName, player);
                            playerStats.Add(weaklink);
                        }

                    }
                }
                Console.WriteLine(ResultMessage(playerStats)); 
            }
            
            await Task.Delay(-1);
        }

        private static PlayerStat GetPlayerStat(string summonerName, JsonElement player)
        {
            PlayerStat playerStat = new PlayerStat();
            playerStat.SummonerName = summonerName;


            JsonElement kills;
            if (player.TryGetProperty("kills", out kills))
            {
                playerStat.Kills = Convert.ToInt32(kills.GetRawText());
            }

            JsonElement deaths;
            if (player.TryGetProperty("deaths", out deaths))
            {
                playerStat.Deaths = Convert.ToInt32(deaths.GetRawText());
            }

            JsonElement assists;
            if (player.TryGetProperty("assists", out assists))
            {
                playerStat.Assists = Convert.ToInt32(assists.GetRawText());
            }

            JsonElement championName;
            if (player.TryGetProperty("championName", out championName))
            {
                playerStat.ChampionName = championName.GetRawText();
            }

            JsonElement participantId;
            if (player.TryGetProperty("participantId", out participantId))
            {
                playerStat.ParticipantId = Convert.ToInt32(participantId.GetRawText());
            }

            JsonElement quadraKills;
            if (player.TryGetProperty("quadraKills", out quadraKills))
            {
                playerStat.QuadraKills = Convert.ToInt32(quadraKills.GetRawText());
            }

            JsonElement pentaKills;
            if (player.TryGetProperty("pentaKills", out pentaKills))
            {
                playerStat.PentaKills = Convert.ToInt32(pentaKills.GetRawText());
            }

            JsonElement totalDamageDealtToChampions;
            if (player.TryGetProperty("totalDamageDealtToChampions", out totalDamageDealtToChampions))
            {
                playerStat.TotalDamageDealtToChampions = Convert.ToInt32(totalDamageDealtToChampions.GetRawText());
            }

            JsonElement spell1Casts;
            if (player.TryGetProperty("spell1Casts", out spell1Casts))
            {
                playerStat.Spell1Casts = Convert.ToInt32(spell1Casts.GetRawText());
            }

            JsonElement visionScore;
            if (player.TryGetProperty("visionScore", out visionScore))
            {
                playerStat.VisionScore = Convert.ToInt32(visionScore.GetRawText());
            }

            JsonElement win;
            if (player.TryGetProperty("win", out win))
            {
                playerStat.Win = Convert.ToBoolean(win.GetRawText());
            }

            JsonElement totalMinionsKilled;
            if (player.TryGetProperty("totalMinionsKilled", out totalMinionsKilled))
            {
                playerStat.TotalMinionsKilled = Convert.ToInt32(totalMinionsKilled.GetRawText());
            }

            JsonElement totalTimeSpentDead;
            if (player.TryGetProperty("totalTimeSpentDead", out totalTimeSpentDead))
            {
                playerStat.TotalTimeSpentDead = Convert.ToInt32(totalTimeSpentDead.GetRawText());
            }

            JsonElement wardsPlaced;
            if (player.TryGetProperty("wardsPlaced", out wardsPlaced))
            {
                playerStat.WardsPlaced = Convert.ToInt32(wardsPlaced.GetRawText());
            }

            JsonElement goldEarned;
            if (player.TryGetProperty("goldEarned", out goldEarned))
            {
                playerStat.GoldEarned = Convert.ToInt32(goldEarned.GetRawText());
            }

            return playerStat;

        }

        private static StringBuilder ResultMessage(List<PlayerStat> playerStats)
        {
            StringBuilder result = new StringBuilder();

            if(playerStats.Count != 0)
            {
                //считаем только между ними
                if (playerStats.Count > 1)
                {
                    foreach(PlayerStat playerStat in playerStats)
                    {
                        result.AppendLine("------------------------");
                        result.AppendLine(playerStat.GetStat().ToString());
                        result.AppendLine("------------------------");
                    }

                }
                else //только одного
                {
                  result = playerStats.First().GetStat();
                }
            }

            


            return result;
        }
    }
}