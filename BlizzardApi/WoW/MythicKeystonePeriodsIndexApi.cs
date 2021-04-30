using BlizzardApi.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.WoW.GameData
{
    /// <summary>
    /// MythicKeystonePeriodsIndexApi call class.
    /// </summary>
    /// <see cref="https://develop.battle.net/documentation/world-of-warcraft/game-data-apis"/>
    public class MythicKeystonePeriodsIndexApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectedRealmId"></param>
        /// <param name="dungeonId"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static async Task<MythicKeystonePeriodsIndex> GetMythicKeystonePeriodsIndex(int connectedRealmId, int dungeonId, int period)
        {
            return await GetMythicKeystonePeriodsIndex(Settings.Locale, Settings.Region, Settings.Token, connectedRealmId, dungeonId, period);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="region"></param>
        /// <param name="token"></param>
        /// <param name="connectedRealmId"></param>
        /// <param name="dungeonId"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static async Task<MythicKeystonePeriodsIndex> GetMythicKeystonePeriodsIndex(Base.Locale locale, Base.Region region, string token, int connectedRealmId, int dungeonId, int period)
        {
            string clientString = $"https://{region.ToDescriptionString()}.api.blizzard.com/data/wow/connected-realm/{connectedRealmId}/mythic-leaderboard/{dungeonId}/period/{period}?namespace=dynamic-{region.ToDescriptionString()}&locale={locale.ToDescriptionString()}&access_token={token}";
            return await Util.RequestHandler.ParseJson<MythicKeystonePeriodsIndex>(clientString, token);
        }

        #region JSON Classes
        /// <summary>
        /// Strongly typed JSON result
        /// </summary>
        public partial class MythicKeystonePeriodsIndex : Util.RequestHandler.IJsonResponse
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

            [JsonProperty("map")]
            public Map Map { get; set; }

            [JsonProperty("period")]
            public long Period { get; set; }

            [JsonProperty("period_start_timestamp")]
            public long PeriodStartTimestamp { get; set; }

            [JsonProperty("period_end_timestamp")]
            public long PeriodEndTimestamp { get; set; }

            [JsonProperty("connected_realm")]
            public ConnectedRealm ConnectedRealm { get; set; }

            [JsonProperty("leading_groups")]
            public List<LeadingGroup> LeadingGroups { get; set; }

            [JsonProperty("keystone_affixes")]
            public List<KeystoneAffixElement> KeystoneAffixes { get; set; }

            [JsonProperty("map_challenge_mode_id")]
            public long MapChallengeModeId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public partial class ConnectedRealm
        {
            [JsonProperty("href")]
            public Uri Href { get; set; }
        }

        public partial class KeystoneAffixElement
        {
            [JsonProperty("keystone_affix")]
            public KeystoneAffixKeystoneAffix KeystoneAffix { get; set; }

            [JsonProperty("starting_level")]
            public long StartingLevel { get; set; }
        }

        public partial class KeystoneAffixKeystoneAffix
        {
            [JsonProperty("key")]
            public ConnectedRealm Key { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }
        }

        public partial class LeadingGroup
        {
            [JsonProperty("ranking")]
            public long Ranking { get; set; }

            [JsonProperty("duration")]
            public long Duration { get; set; }

            [JsonProperty("completed_timestamp")]
            public long CompletedTimestamp { get; set; }

            [JsonProperty("keystone_level")]
            public long KeystoneLevel { get; set; }

            [JsonProperty("members")]
            public List<Member> Members { get; set; }
        }

        public partial class Member
        {
            [JsonProperty("profile")]
            public Profile Profile { get; set; }

            [JsonProperty("faction")]
            public Faction Faction { get; set; }

            [JsonProperty("specialization")]
            public Specialization Specialization { get; set; }
        }

        public partial class Faction
        {
            [JsonProperty("type")]
            public TypeEnum Type { get; set; }
        }

        public partial class Profile
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("realm")]
            public Realm Realm { get; set; }
        }

        public partial class Realm
        {
            [JsonProperty("key")]
            public ConnectedRealm Key { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("slug")]
            public string Slug { get; set; }
        }

        public partial class Specialization
        {
            [JsonProperty("key")]
            public ConnectedRealm Key { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }
        }

        public partial class Links
        {
            [JsonProperty("self")]
            public ConnectedRealm Self { get; set; }
        }

        public partial class Map
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }
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
        #endregion JSON Classes
    }
}