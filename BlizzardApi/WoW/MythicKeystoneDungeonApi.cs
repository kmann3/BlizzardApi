using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.WoW.GameData
{
    public class MythicKeystoneDungeonApi
    {
        public static async Task<Tuple<HttpStatusCode,MythicKeystoneDungeon>> GetMythicKeystoneDungeon(string token, Base.Region region, Base.Locale locale)
        {
            string clientString = $"https://{region.ToString()}.api.blizzard.com/data/wow/mythic-keystone/dungeon/index?namespace=dynamic-{region.ToString()}&locale={locale.ToString()}&access_token={token}";

            IRestResponse response = await Util.RequestHandler.SendRequest(clientString, token);
            MythicKeystoneDungeon returnValue = new();

            // Anything in the 200 range is considered an acceptable response but anything other than 200 exactly means you should investigate further. Could be cached, could be delayed, could be all sorts of things...
            if ((int)response.StatusCode == 200)
            {
                returnValue = JsonConvert.DeserializeObject<MythicKeystoneDungeon>(response.Content);
            }

            return new Tuple<HttpStatusCode, MythicKeystoneDungeon>(response.StatusCode,returnValue);
        }

        public partial class MythicKeystoneDungeon
        {
            [JsonProperty("_links")]
            public Links Links { get; set; }

            [JsonProperty("dungeons")]
            public List<Dungeon> Dungeons { get; set; }

            public override string ToString()
            {
                if (Dungeons != null)
                {
                    return $"Links:{Links.Self.Href.ToString()}" + System.Environment.NewLine + String.Join(", ", Dungeons);
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
    }
}
