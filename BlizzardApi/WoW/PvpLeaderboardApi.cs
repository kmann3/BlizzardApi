using BlizzardApi.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.WoW.GameData
{
    /// <summary>
    /// Pvp Leaderboard
    /// </summary>
    /// <see cref="https://develop.battle.net/documentation/world-of-warcraft/game-data-apis"/>
    public class PvpLeaderboardApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pvpSeasonId"></param>
        /// <param name="pvpBracket"></param>
        /// <returns></returns>
        public static async Task<JSON_PvpLeaderboard> GetPvpLeaderboardApi(int pvpSeasonId, PvpBracket pvpBracket)
        {
            return await GetPvpLeaderboardApi(Settings.Locale, Settings.Region, Settings.Token, pvpSeasonId, pvpBracket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="region"></param>
        /// <param name="token"></param>
        /// <param name="pvpSeasonId"></param>
        /// <param name="pvpBracket"></param>
        /// <returns></returns>
        public static async Task<JSON_PvpLeaderboard> GetPvpLeaderboardApi(Base.Locale locale, Base.Region region, string token, int pvpSeasonId, PvpBracket pvpBracket)
        {
            string clientString = $"https://{region.ToDescriptionString()}.api.blizzard.com/data/wow/pvp-season/{pvpSeasonId}/pvp-leaderboard/{pvpBracket.ToDescriptionString()}?namespace=dynamic-{region.ToDescriptionString()}&locale={locale.ToDescriptionString()}&access_token={token}";
            return await Util.RequestHandler.ParseJson<JSON_PvpLeaderboard>(clientString, token);
        }

        public enum PvpBracket
        {
            [Description("3v3")]
            Three,

            [Description("2v2")]
            Two,

            [Description("rbg")]
            RBG            
        }

        public partial class JSON_PvpLeaderboard : RequestHandler.IJsonResponse
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

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("bracket")]
            public JSON_Bracket Bracket { get; set; }

            [JsonProperty("entries")]
            public List<JSON_Entry> Entries { get; set; }

            public partial class JSON_Bracket
            {
                [JsonProperty("id")]
                public long Id { get; set; }

                [JsonProperty("type")]
                public string Type { get; set; }
            }

            public partial class JSON_Entry
            {
                [JsonProperty("character")]
                public JSON_Character Character { get; set; }

                [JsonProperty("faction")]
                public JSON_Faction Faction { get; set; }

                [JsonProperty("rank")]
                public long Rank { get; set; }

                [JsonProperty("rating")]
                public long Rating { get; set; }

                [JsonProperty("season_match_statistics")]
                public JSON_SeasonMatchStatistics SeasonMatchStatistics { get; set; }

                [JsonProperty("tier")]
                public JSON_Season Tier { get; set; }
            }

            public partial class JSON_Character
            {
                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("id")]
                public long Id { get; set; }

                [JsonProperty("realm")]
                public JSON_Realm Realm { get; set; }
            }

            public partial class JSON_Realm
            {
                [JsonProperty("key")]
                public JSON_Self Key { get; set; }

                [JsonProperty("id")]
                public long Id { get; set; }

                [JsonProperty("slug")]
                public string Slug { get; set; }
            }

            public partial class JSON_Self
            {
                [JsonProperty("href")]
                public Uri Href { get; set; }
            }

            public partial class Faction
            {
                [JsonProperty("type")]
                public TypeEnum Type { get; set; }
            }

            public partial class JSON_SeasonMatchStatistics
            {
                [JsonProperty("played")]
                public long Played { get; set; }

                [JsonProperty("won")]
                public long Won { get; set; }

                [JsonProperty("lost")]
                public long Lost { get; set; }
            }

            public partial class JSON_Season
            {
                [JsonProperty("key")]
                public JSON_Self Key { get; set; }

                [JsonProperty("id")]
                public long Id { get; set; }
            }

            public partial class JSON_Links
            {
                [JsonProperty("self")]
                public JSON_Self Self { get; set; }
            }
        }

        

        public enum TypeEnum { Alliance, Horde };

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class TypeEnumConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "ALLIANCE":
                        return TypeEnum.Alliance;
                    case "HORDE":
                        return TypeEnum.Horde;
                }
                throw new Exception("Cannot unmarshal type TypeEnum");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (TypeEnum)untypedValue;
                switch (value)
                {
                    case TypeEnum.Alliance:
                        serializer.Serialize(writer, "ALLIANCE");
                        return;
                    case TypeEnum.Horde:
                        serializer.Serialize(writer, "HORDE");
                        return;
                }
                throw new Exception("Cannot marshal type TypeEnum");
            }

            public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
        }
    }
}
