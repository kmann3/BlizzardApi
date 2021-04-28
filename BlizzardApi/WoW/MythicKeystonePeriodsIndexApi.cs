using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.WoW.GameData
{
    public class MythicKeystonePeriodsIndexApi
    {
        public static async Task<Tuple<HttpStatusCode, MythicKeystonePeriodsIndex>> GetMythicKeystonePeriodsIndex(string token, Base.Region region, int connectedRealmId, int dungeonId, int period, Base.Locale locale)
        {
            string clientString = $"https://{region.ToString()}.api.blizzard.com/data/wow/connected-realm/{connectedRealmId}/mythic-leaderboard/{dungeonId}/period/{period}?namespace=dynamic-{region.ToString()}&locale={locale.ToString()}&access_token={token}";

            var client = new RestClient(clientString);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", $"Bearer {token}");
            IRestResponse response = await client.ExecuteAsync(request);
            MythicKeystonePeriodsIndex returnValue = new();

            // Anything in the 200 range is considered an ok response but anything other than 200 means you should investigate further.
            if ((int)response.StatusCode == 200)
            {
                returnValue = JsonConvert.DeserializeObject<MythicKeystonePeriodsIndex>(response.Content);
            }

            //return new Tuple<HttpStatusCode, MythicKeystonePeriodsIndex>(response.StatusCode, returnValue);
            throw new NotImplementedException();
        }

        public partial class MythicKeystonePeriodsIndex
        {
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
    }
}