using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace BlizzardApi.Util
{
    public class RequestHandler
    {
        /// <summary>
        /// Helper for sending requests.
        /// </summary>
        /// <param name="clientString">URL / Query to send.</param>
        /// <param name="token">Your access token.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Used by all the API calls
        /// </summary>
        /// <typeparam name="T">Type of API call (e.g. MythicKeystoneDungeon)</typeparam>
        /// <param name="clientString">URL / Query to send.</param>
        /// <param name="token">Your access token.</param>
        /// <returns></returns>
        public static async Task<T> ParseJson<T>(string clientString, string token) where T : IJsonResponse, new()
        {
            IRestResponse response = await RequestHandler.SendRequest(clientString, token);
            T returnValue = JsonConvert.DeserializeObject<T>(response.Content);
            if (returnValue == null) returnValue = new T();
            returnValue.JsonData = response.Content ?? null;
            returnValue.HttpStatusCode = response.StatusCode;
            
            return returnValue;
        }

        /// <summary>
        /// Interfaced used to add response data to the JSON classes.
        /// Primarily needed to help sanity check responses.
        /// </summary>
        public interface IJsonResponse
        {
            public string JsonData { get; set; }
            public HttpStatusCode HttpStatusCode {get;set;}
        }
    }
}
