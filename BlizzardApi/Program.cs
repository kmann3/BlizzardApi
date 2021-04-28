using System;
using BlizzardApi.WoW;
using BlizzardApi.WoW.GameData;
using System.Configuration;

namespace BlizzardApi
{
    class Program
    {
        public static void Main()
        {
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string secret = ConfigurationManager.AppSettings["Secret"];
            string accessToken = Base.GetAuthToken(WoW.Base.Region.US, clientId, secret).Result;

            var x = MythicKeystoneDungeonApi.GetMythicKeystoneDungeon(accessToken, Base.Region.US, Base.Locale.EN_US).Result;

            Console.WriteLine("Status Code: " + x.Item1.ToString());
            Console.WriteLine("Result: " + x.Item2.ToString());
            Console.Read();
        }
    }
}
