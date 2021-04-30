using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BlizzardApi.Util;

namespace BlizzardApi.WoW.GameData
{
    /// <summary>
    /// MythicKeystoneDguneonApi call class.
    /// </summary>
    /// <see cref="https://develop.battle.net/documentation/world-of-warcraft/game-data-apis"/>
    public class MythicKeystoneDungeonApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<MythicKeystoneDungeon> GetMythicKeystoneDungeon()
        {
            return await GetMythicKeystoneDungeon(Settings.Locale, Settings.Region, Settings.Token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="region"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<MythicKeystoneDungeon> GetMythicKeystoneDungeon(Base.Locale locale, Base.Region region, string token)
        {
            string clientString = $"https://{region.ToDescriptionString()}.api.blizzard.com/data/wow/mythic-keystone/dungeon/index?namespace=dynamic-{region.ToDescriptionString()}&locale={locale.ToDescriptionString()}&access_token={token}";
            return await Util.RequestHandler.ParseJson<MythicKeystoneDungeon>(clientString, token);
        }

        #region JSON Classes
        /// <summary>
        /// Strongly typed JSON result
        /// </summary>
        public partial class MythicKeystoneDungeon : Util.RequestHandler.IJsonResponse
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
            public Links Links { get; set; }

            [JsonProperty("dungeons")]
            public List<Dungeon> Dungeons { get; set; }

            public override string ToString()
            {
                if (Dungeons != null)
                {
                    return String.Join(", ", Dungeons);
                } else
                {
                    return "Empty";
                }
            }
        }

        public partial class Dungeon
        {
            [JsonProperty("key")]
            public Self Key { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            public override string ToString()
            {
                return $"[ID:{Id}] {Name}";
            }
        }

        public partial class Self
        {
            [JsonProperty("href")]
            public Uri Href { get; set; }
        }

        public partial class Links
        {
            [JsonProperty("self")]
            public Self Self { get; set; }
        }
        #endregion JSON Classes
    }
}
