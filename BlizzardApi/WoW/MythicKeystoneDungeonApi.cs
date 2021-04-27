﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace BlizzardApi.WoW.GameData
{
    public class MythicKeystoneDungeonApi
    {
        public static async Task<Tuple<HttpStatusCode,MythicKeystoneDungeon>> GetMythicKeystoneDungeon(string token, Base.Region region, Base.Locale locale)
        {
            string clientString = $"https://{region.ToString()}.api.blizzard.com/data/wow/mythic-keystone/dungeon/index?namespace=dynamic-{region.ToString()}&locale={locale.ToString()}&access_token={token}";

            var client = new RestClient(clientString);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", $"Bearer {token}");
            IRestResponse response = await client.ExecuteAsync(request);

            MythicKeystoneDungeon returnValue = JsonConvert.DeserializeObject<MythicKeystoneDungeon>(response.Content);

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
                    return $"Links:{Links}" + System.Environment.NewLine + $"{Dungeons.Count}";
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
