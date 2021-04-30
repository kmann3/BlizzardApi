using Newtonsoft.Json;
using RestSharp;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BlizzardApi.Util;

namespace BlizzardApi.WoW
{
    public class Base
    {
        public enum Locale
        {
            [Description("en_US")]
            EN_US
        }

        public enum Region
        {
            APAC,
            CN,
            EU,
            US
        }

        private static string GetAuthorizeUri(Region r)
        {
            return r switch
            {
                Region.APAC or Region.EU or Region.US => $"https://{r.ToDescriptionString()}.battle.net/oauth/authorize",
                Region.CN => "https://www.battlenet.com.cn/oauth/authorize",
                _ => throw new Exception("Unknown region for auth."),
            };
        }

        private static string GetTokenUri(Region r)
        {
            return r switch
            {
                Region.APAC or Region.EU or Region.US => $"https://{r.ToDescriptionString()}.battle.net/oauth/token",
                Region.CN => "https://www.battlenet.com.cn/oauth/token",
                _ => throw new Exception("Unknown region for token."),
            };
        }

        public static async Task<string> GetAuthToken(Region region, string clientId, string clientSecret)
        {

            var client = new RestSharp.RestClient(GetTokenUri(region));
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter($"application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            return tokenResponse.access_token;
        }

        private class AccessTokenResponse
        {
            public string access_token { get; set; }
        }

    }
}
