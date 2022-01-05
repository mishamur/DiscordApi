using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Text.Json;
using MingweiSamuel;
using MingweiSamuel.Camille.Enums;


namespace LeagueApi
{
    class myRiotApi
    {
        static async Task MainAsync()
        {
            const string api_key = "RGAPI-8939977c-1bcc-47ba-afc8-98d90f6b4f3d";
            var api = MingweiSamuel.Camille.RiotApi.NewInstance("api_key");

            var sum = api.SummonerV4.GetBySummonerName(Region.RU, "neadecvatt228");
            Console.WriteLine(sum.Puuid);
            Console.WriteLine(sum.SummonerLevel);
            Console.WriteLine(sum.Name);


            try
            {

                Regex regex = new Regex(@"(RU_\d+)");
                List<string> matches;

                {
                    WebRequest webRequest = WebRequest.Create($"https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/{sum.Puuid}/ids?start=0&count=20&api_key={api_key}");
                    WebResponse webResponse = await webRequest.GetResponseAsync();


                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            matches = regex.Matches(reader.ReadToEnd()).ToList().Select((x) => x.Value).ToList();
                        }
                    }
                    webResponse.Close();
                }

                matches.ForEach((x) => Console.WriteLine(x));

                string textPath = "C:\\Users\\misha\\source\\repos\\DiscordApi\\LeagueApi\\text.txt";
                try
                {
                    using (StreamWriter sw = new StreamWriter(textPath, false, System.Text.Encoding.Default))
                    {
                        matches.ForEach(x => sw.WriteLine(x));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                string lastMatch = matches.First();


                {
                    WebRequest webRequest = WebRequest.Create($"https://europe.api.riotgames.com/lol/match/v5/matches/{lastMatch}?api_key={api_key}");
                    WebResponse webResponse = await webRequest.GetResponseAsync();
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            //Console.WriteLine(reader.ReadToEnd());
                            JsonProperty json = JsonSerializer.Deserialize<JsonProperty>(reader.ReadToEnd());
                            Console.WriteLine(json);
                        }
                    }
                    webResponse.Close();

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            await Task.Delay(-1);
        }

    }
}
