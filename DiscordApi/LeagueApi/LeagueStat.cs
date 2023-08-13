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
using System.Threading;
using DiscordApi.Drawing;

namespace DiscordApi.LeagueApi
{
    static class LeagueStat
    {
        private const string api_key = "RGAPI-1d7b6e5e-5230-4b10-a935-047f80b76d23";
        public static async Task<StringBuilder> MainAsync(ulong authorId)
        {
            var api = MingweiSamuel.Camille.RiotApi.NewInstance($"{api_key}");
            StringBuilder resultMes = new StringBuilder();

            //////////////////////////////////////////////////
            List<User> users = null;
            List<User> friends = new List<User>();
            
            using (ApplicationContext db = new ApplicationContext())
            {

                users = db.Users.Select(u => u).Where(u => u.AuthorId == authorId).ToList();
            }

            if(users != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    friends.AddRange(db.Users.Select(x => x).Where(x => x.GuildId == users.First().GuildId));
                }


                var matches = api.MatchV5.GetMatchIdsByPUUID(Region.Europe, users.First().Puuid);

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
                StringBuilder gameStat = new StringBuilder((await GetGameStat(json)).ToString());
                if(gameStat != null)
                    resultMes.AppendLine(gameStat.ToString());
                

            }

            return await Task.FromResult<StringBuilder>(resultMes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="summonerName"></param>
        /// <returns>path to image</returns>
        public static async Task<string> GetWintate(string summonerName)
        {
            //StringBuilder result = new StringBuilder();
            string puuid = GetPuuidByName(summonerName);
            if(puuid != null)
            {
                try
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        User findUser = null;
                        foreach(var user in db.Users)
                        {
                            if(user.Puuid == puuid)
                            {
                                findUser = user;
                                break;
                            }
                        }
                        if (findUser == null)
                        {
                            User user = new User { Puuid = puuid, SummName = summonerName };
                            db.Users.Add(user);
                            Console.WriteLine("добавил юзера");
                        }
                        else
                        {
                            if (!summonerName.Equals(findUser.SummName))
                            {
                                findUser.SummName = summonerName;
                                db.Users.Update(findUser);
                            }
                        }
                        db.SaveChanges();
                        Console.WriteLine("save db");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + " " + ex.StackTrace);
                    return "ошибка";
                }
                


                var api = MingweiSamuel.Camille.RiotApi.NewInstance($"{api_key}");
                if(api != null)
                {
                    string[] matches = null;
                    int startCountMatches = 0;
                    
                    bool needFindMatches = true;
                    do
                    {
                       
                        matches = api.MatchV5.GetMatchIdsByPUUID(Region.Europe, puuid, startCountMatches, 100);
                        Console.WriteLine(matches);
                        foreach (string matchId in matches)
                        {
                            Console.WriteLine(matchId);
                            PlayerGameStat playerGameStat = null;
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                playerGameStat = db.PlayerGameStats.FirstOrDefault(x => (x.MatchId == matchId &&
                                x.UserPuuid == puuid));
                            }
                            if(playerGameStat != null)
                            {
                                //needFindMatches = false;
                                continue;
                            }

                            var match = api.MatchV5.GetMatch(Region.Europe, matchId);
                            if (match == null)
                                continue;

                            Dictionary<string, object>.ValueCollection valueCollection = match._AdditionalProperties.Values;
                            
                            string gameInfo = valueCollection.ToList().Last().ToString();
                            JsonDocument json = JsonSerializer.Deserialize<JsonDocument>(gameInfo);
                            Console.WriteLine( GetGameStat(json).Result.ToString());
                            GameStat gameStat = GetGameStat(json).Result;

                            JsonElement jsonParticipants;
                            //берём участников игры
                            bool isParticipants = json.RootElement.TryGetProperty("participants", out jsonParticipants);
                            if (isParticipants)
                            {                              
                                //преобразуем json в массив данных о игроках
                                JsonElement.ArrayEnumerator particArray = jsonParticipants.EnumerateArray();
                              
                                foreach (var player in particArray)
                                {
                                    JsonElement jsonSummName;
                       
                                    if (player.TryGetProperty("summonerName", out jsonSummName))
                                    {
                                        string summonerNames = jsonSummName.GetRawText().ToString();
                                            //если нашли в массиве нашего игрока, считаем и победы и
                                            //записываем статистику в базу данных
                                            if (('"' + summonerName + '"').Equals(summonerNames))
                                            {
                                                PlayerStat playerStat = GetPlayerStat(summonerName, player);
                                                try
                                                {
                                                    using (ApplicationContext db = new ApplicationContext())
                                                    {
                                                        PlayerGameStat pgs = new PlayerGameStat()
                                                        {
                                                            MatchId = matchId,
                                                            GameStat = gameStat,
                                                            PlayerStat = playerStat,
                                                            UserPuuid = puuid
                                                        };
                                                        db.PlayerGameStats.Add(pgs);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                    Console.WriteLine(ex.StackTrace);
                                                }
                                                
                                                break;
                                            }                                        
                                    }
                                }
                            }
                        }
                        Console.WriteLine("--------------------------");
                        startCountMatches += 100;
                    } while (matches.Length != 0 && needFindMatches);

                    List<PlayerGameStat> collectionGamesCurPlayer = null;
                    using(ApplicationContext db = new ApplicationContext())
                    {
                        collectionGamesCurPlayer = db.PlayerGameStats.Select(x => x).Where(x => x.User.Puuid == puuid).ToList();                        
                    }
                    if(collectionGamesCurPlayer != null)
                        return await Task.FromResult(DrawingChart.DrawAndSave(collectionGamesCurPlayer));
                    return await Task.FromResult("проблема с кол-вом игр");
                }
            }
            else
            {
                return await Task.FromResult("такого игрока не существует");
            }
            return await Task.FromResult("что-то отвалилось");

        }

        private static Task<GameStat> GetGameStat(JsonDocument gameStat)
        {
            GameStat resultGameStat = new GameStat();
            JsonElement gameStartTime;
            bool isGameStartTime = gameStat.RootElement.TryGetProperty("gameStartTimestamp",out gameStartTime);

            JsonElement gameEndTime;
            bool isGameEndTime = gameStat.RootElement.TryGetProperty("gameEndTimestamp", out gameEndTime);

            if( isGameStartTime)
            {
                resultGameStat.gameStartTimestamp = gameStartTime.GetRawText().ToString();
               
                if (isGameEndTime)
                {
                    resultGameStat.gameEndTimestamp = gameEndTime.GetRawText().ToString();
                }
                
            }
            return Task.FromResult(resultGameStat);

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
