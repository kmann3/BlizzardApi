using BlizzardApi.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using static BlizzardApi.WoW.GameData.PvpLeaderboardApi;
using static BlizzardApi.WoW.GameData.PvpLeaderboardApi.JSON_PvpLeaderboard;

namespace BlizzardApi.WoW.GameData
{
    /// <summary>
    /// Pvp Leaderboard
    /// </summary>
    /// <see cref="https://develop.battle.net/documentation/world-of-warcraft/game-data-apis"/>
    public class PvpLeaderboardIndexApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<JSON_PvpLeaderboardIndex> GetPvpLeaderboardIndexApi()
        {
            //return await GetPvpLeaderboardIndexApi(Settings.Locale, Settings.Region, Settings.Token);
            throw new NotImplementedException();
        }

        public partial class JSON_PvpLeaderboardIndex : RequestHandler.IJsonResponse
        {
            /// <summary>
            /// Returned JSON data, raw.
            /// If the query failed JsonData will be an empty string.
            /// If they change the JSON results and the class isn't populating, this is where to start on fixing it.
            /// </summary>
            public string JsonData { get; set; }

            /// <summary>
            /// Returned StatusCode. Check for (int)200 for OK response to know if this class is populated.
            /// If you goofed your token, you will get (int)401 for Unauthorized.
            /// If the URL is wrong, somehow, you will get (int)404 for Not Found.
            /// If you the parameters sent are incorrect you will get (int)403 Forbidden
            /// </summary>
            public HttpStatusCode HttpStatusCode { get; set; }

            [JsonProperty("_links")]
            public JSON_Links Links { get; set; }

            [JsonProperty("season")]
            public JSON_Season Season { get; set; }

            [JsonProperty("leaderboards")]
            public List<JSON_Leaderboard> Leaderboards { get; set; }

            public partial class JSON_Leaderboard
            {
                [JsonProperty("key")]
                public JSON_Self Key { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("id")]
                public long Id { get; set; }
            }

            public partial class JSON_Self
            {
                [JsonProperty("href")]
                public Uri Href { get; set; }
            }

            public partial class JSON_Links
            {
                [JsonProperty("self")]
                public JSON_Self Self { get; set; }
            }

            public partial class JSON_Season
            {
                [JsonProperty("key")]
                public JSON_Self Key { get; set; }

                [JsonProperty("id")]
                public long Id { get; set; }
            }
        }
    }
}
