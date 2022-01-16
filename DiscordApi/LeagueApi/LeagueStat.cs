using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueApi.Models;
using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordApi.LeagueApi.Models;

namespace DiscordApi.LeagueApi
{
    static class LeagueStat
    {
        private const string api_key = "RGAPI-ca925b6d-0c98-4e60-be6d-273112bb48ca";
        public static async Task<StringBuilder> MainAsync(ulong authorId)
        {
            var api = MingweiSamuel.Camille.RiotApi.NewInstance($"{api_key}");
            StringBuilder resultMes = new StringBuilder();

            //////////////////////////////////////////////////
            User user = null;
            List<User> friends = new List<User>();
            
            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Users.Find(authorId);
            }

            if(user != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    friends.AddRange(db.Users.Select(x => x).Where(x => x.GuildId == user.GuildId));
                }


                var matches = api.MatchV5.GetMatchIdsByPUUID(Region.Europe, user.Puuid);

                matches.ToList().ForEach(x => Console.WriteLine(x));

                //
                var match = api.MatchV5.GetMatch(Region.Europe, matches.First());
                //
                //
                Dictionary<string, object>.ValueCollection valueCollection = match._AdditionalProperties.Values;
                //

                string last = valueCollection.ToList().Last().ToString();
                //
                // Console.WriteLine(match._AdditionalProperties.Values.ElementAt(0).GetType());


                JsonDocument json = JsonSerializer.Deserialize<JsonDocument>(last);

                JsonElement jsonParticipants;
                bool isParticipants = json.RootElement.TryGetProperty("participants", out jsonParticipants);
                //информация о каждом чемпионе
                if (isParticipants)
                {
                    //Console.WriteLine("jsonParticipants " + jsonParticipants.GetRawText());
                    JsonElement.ArrayEnumerator particArray = jsonParticipants.EnumerateArray();

                    //  particArray.ToList().ForEach(x => Console.WriteLine(x));
                    //  Console.WriteLine("----------------------------------------------");


                    List<PlayerStat> playerStats = new List<PlayerStat>();
                    foreach (var player in particArray)
                    {
                        JsonElement jsonSummName;

                        if (player.TryGetProperty("summonerName", out jsonSummName))
                        {
                            string summonerName = jsonSummName.GetRawText();

                            foreach (var f in friends)
                            {
                                if (('"' + f.SummName + '"').Equals(summonerName))
                                {
                                    PlayerStat playerStat = GetPlayerStat(summonerName, player);
                                    playerStats.Add(playerStat);
                                }
                            }
                        }
                    }
                    resultMes = ResultMessage(playerStats);
                    //Console.WriteLine(ResultMessage(playerStats));
                }

                //информация о игре
                StringBuilder gameStat = await GetGameStat(json);
                if(gameStat != null)
                    resultMes.AppendLine(gameStat.ToString());
                

            }

            return await Task.FromResult<StringBuilder>(resultMes);
        }

        private async static Task<StringBuilder> GetGameStat(JsonDocument gameStat)
        {
            StringBuilder resultGameStat = new StringBuilder();
            JsonElement gameStartTime;
            bool isGameStartTime = gameStat.RootElement.TryGetProperty("gameStartTimestamp",out gameStartTime);

            JsonElement gameEndTime;
            bool isGameEndTime = gameStat.RootElement.TryGetProperty("gameEndTimestamp", out gameEndTime);

            if(isGameEndTime && isGameStartTime)
            {
                DateTime timeStart =  DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(gameStartTime.GetRawText().ToString()));
                DateTime timeEnd = DateTime.UnixEpoch.AddMilliseconds(Convert.ToDouble(gameEndTime.GetRawText().ToString()));
                TimeSpan tsStart = TimeSpan.Parse(timeStart.ToString("HH:mm:ss"));
                TimeSpan tsEnd = TimeSpan.Parse(timeEnd.ToString("HH:mm:ss"));

                resultGameStat.AppendLine("Сведения о матче");
                resultGameStat.AppendLine($"Время начала: {timeStart.Date.ToLongDateString()} {tsStart}");
                resultGameStat.AppendLine($"Время окончания: {timeEnd.Date.ToLongDateString()} {tsEnd}");
                resultGameStat.AppendLine($"Продолжительность игры: {(tsEnd - tsStart)}");

                JsonElement jsonGameMode;
                bool isGameMode = gameStat.RootElement.TryGetProperty("gameMode", out jsonGameMode);
                if (isGameMode)
                    resultGameStat.AppendLine($"Мод: {jsonGameMode.GetRawText().ToString()}");
            }
            return await Task.FromResult(resultGameStat);

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

            JsonElement spell4Casts;
            if(player.TryGetProperty("spell4Casts", out spell4Casts))
            {
                playerStat.Spell4Casts = Convert.ToInt32(spell4Casts.GetRawText());
            }

            JsonElement timeCCingOthers;
            if(player.TryGetProperty("timeCCingOthers", out timeCCingOthers))
            {
                playerStat.TimeCCingOthers = Convert.ToInt32(timeCCingOthers.GetRawText());
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

            JsonElement neutralMinionsKilled;
            if(player.TryGetProperty("neutralMinionsKilled", out neutralMinionsKilled))
            {
                playerStat.NeutralMinionsKilled = Convert.ToInt32(neutralMinionsKilled.GetRawText());
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

            if (playerStats.Count != 0)
            {
                //считаем только между ними
                if (playerStats.Count > 1)
                {
                    foreach (PlayerStat playerStat in playerStats)
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

        public static string GetPuuidByName(string name)
        {
            var api = MingweiSamuel.Camille.RiotApi.NewInstance($"{api_key}");

            string result;
            try
            {
                result = api.SummonerV4.GetBySummonerName(Region.RU, name).Puuid;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;

        }
    }
}
