using BlizzardApi.WoW;
using System;
using System.Configuration;

namespace BlizzardApi
{
    class Program
    {
        public static void Main()
        {
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
            GetAndSaveJsonDataForApi();

            //==============================
            // Example usage
            //==============================
            //var x = MythicKeystoneDungeonApi.GetMythicKeystoneDungeon().Result;
            //if((int)x.HttpStatusCode == 200)
            //{
            //    Console.WriteLine("Result: " + x.ToString());
            //    Console.WriteLine("JSON: " + x.JsonData);
            //} else
            //{
            //    Console.WriteLine("FAILED: MythicKeystoneDungeonApi");
            //}

            //Console.WriteLine("Tasks complete. fin");
            //Console.Read();
        }

        /// <summary>
        /// Helper method to help me pull JSON data to generate classes.
        /// </summary>
        public static void GetAndSaveJsonDataForApi()
        {
            // Do stuff
            // Save JSON to file
            // Open file.
        }
    }
}
