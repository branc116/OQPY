using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace OQPYHelper.AuthHelper
{
    public static class Auth
    {
        public static bool ValidateMasterAdminKey(string key)
        {
#if DEBUG
            return true;
#else
            return !string.IsNullOrWhiteSpace(key) && Environment.GetEnvironmentVariable("OQPYMasterAdminKey") == key;
#endif
        }
    }
    public class FacebookAcessToken
    {
        public FacebookAcessToken()
        {
        }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiresIn { get; set; }
    }
    public class FacebookProfile
    {
        public FacebookProfile()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
    public static class FacebookHelpers
    {

        // The Facebook App Id
        public static readonly string FacebookAppId = Environment.GetEnvironmentVariable("OQPYFacebookAppId") ?? throw new NullReferenceException("OQPYFacebookAppId");

        // The Facebook App Secret
        public static readonly string FacebookAppSecret = Environment.GetEnvironmentVariable("OQPYFacebookAppSecret") ?? throw new NullReferenceException("OQPYFacebookAppSecret");
        public static async Task<bool> ValidateAccessToken(string accessToken)
        {
           
                var uri = GetUri("https://graph.facebook.com/debug_token",
                    ("input_token", accessToken),
                    ("access_token", $"{FacebookAppId}|{FacebookAppSecret}"));
            try
            {
                var res = await FacebookRequest<object>(uri).ConfigureAwait(false);
                return (((dynamic)res)?.data)?.is_valid;
            }catch(Exception ex)
            {
                throw new Exception(uri.ToString(), ex);
            }
        }
        public static Uri GetUri(string endPoint, params (string, string)[] queryParams)
        {
            var queryString = string.Empty;
            for (int i = 0; i < queryParams.Length; i++)
            {
                queryString += $"{queryParams[i].Item1}={queryParams[i].Item2}{(queryParams.Length - 1 == i ? string.Empty : "&")}";
            }
            var builder = new UriBuilder(endPoint)
            {
                Query = queryString
            };
            return builder.Uri;
        }
        private static async Task<T> FacebookRequest<T>(Uri uri)
        {
            string json;
            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync(uri).ConfigureAwait(false);
            }

            try
            {
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Unable to deserialize the Facebook response.", ex);
            }
        }

        public static async Task<string> GetFacebookProfileName(string accessToken)
        {
            var uri = GetUri("https://graph.facebook.com/v2.9/me",
                ("fields", "id,name"),
                ("access_token", accessToken));

            var res = await FacebookRequest<FacebookProfile>(uri);
            return res.Name;
        }
        public static async Task<string> GetFacebookProfileId(string accessToken)
        {
            var uri = GetUri("https://graph.facebook.com/v2.9/me",
                ("fields", "id,name"),
                ("access_token", accessToken));

            var res = await FacebookRequest<FacebookProfile>(uri);
            return res.Id;
        }
        public static async Task<FacebookProfile> GetFacebookProfile(string accessToken)
        {
            var uri = GetUri("https://graph.facebook.com/v2.9/me",
                ("fields", "id,name"),
                ("access_token", accessToken));

            var res = await FacebookRequest<FacebookProfile>(uri);
            return res;
        }
    }
}
