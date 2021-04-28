using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.Util
{
    public class RequestHandler
    {
        public static async Task<IRestResponse> SendRequest(string clientString, string token)
        {
            var client = new RestClient(clientString);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", $"Bearer {token}");
            IRestResponse response = await client.ExecuteAsync(request);

            return response;
        }
    }
}
