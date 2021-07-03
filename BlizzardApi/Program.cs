using BlizzardApi.WoW;
using System;
using System.Configuration;
using BlizzardApi.WoW.GameData;
using System.Net;
using System.IO;

namespace BlizzardApi
{
    class Program
    {
        public static void Main()
        {
            // 1 - Get list of people in PvP
            // 2 - Get talent selections



            // Bookmark for me.
            //https://develop.battle.net/documentation/world-of-warcraft/game-data-apis

            //==============================
            // Base stuff needed
            //==============================
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string secret = ConfigurationManager.AppSettings["Secret"];
            string accessToken = Base.GetAuthToken(Base.Region.US, clientId, secret).Result;

            //==============================
            // Stuff to make my life easier and methods less... thick when calling. Saves on carpal tunnel.
            // If these are not set AND none are specific during the method call, US defaults will be used.
            //==============================
            WoW.Settings.AssignSettings(Base.Locale.EN_US, Base.Region.US, accessToken);
            //==============================

            // Helper method for me pulling json so I can convert it to a strongly typed class.
            //GetAndSaveJsonDataForApi(accessToken);


            //==============================
            // Example usage
            //==============================
            var x = PvpLeaderboardApi.GetPvpLeaderboardApi(27, PvpLeaderboardApi.PvpBracket.Three).Result;
            if ((int)x.HttpStatusCode == 200)
            {
                Console.WriteLine("Result: " + x.ToString());
                Console.WriteLine("JSON: " + x.JsonData);
            }
            else
            {
                Console.WriteLine("FAILED: PvpLeaderboardApi");
            }

            Console.WriteLine("Tasks complete. fin");
            Console.Read();
        }

        /// <summary>
        /// Helper method to help me pull JSON data to generate classes.
        /// This is purely to help me create code and won't be useful for anyone else actually using the API's.
        /// </summary>
        public static async void GetAndSaveJsonDataForApi(string token)
        {
            string clientString = $"https://us.api.blizzard.com/data/wow/pvp-season/index?namespace=dynamic-us&locale=en_US&access_token={token}";

            var x = await Util.RequestHandler.ParseJson<Foo>(clientString, token);

            string _namespace = "BlizzardApi.WoW.GameData";
            string _className = ("PvP Seasons Index ");

            //string output = _namespace + System.Environment.NewLine + _className.Remove(' ') + System.Environment.NewLine + x.JsonData;
            string output = $"{_namespace}\r\n{_className.Replace(" ", "")}\r\n----------------\r\n{x.JsonData}";
            File.WriteAllText("json_output.txt", output);
            System.Diagnostics.ProcessStartInfo startInfo = new(@"C:\Program Files\Notepad++\notepad++.exe");
            startInfo.Arguments = "json_output.txt";
            System.Diagnostics.Process.Start(startInfo);
        }

        public class Foo : Util.RequestHandler.IJsonResponse
        {
            public string JsonData { get; set; }
            public HttpStatusCode HttpStatusCode { get; set; }
        }
    }
}
