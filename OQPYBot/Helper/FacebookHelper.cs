using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OQPYBot.Helper
{
    public class FacebookHelper
    {
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

        class FacebookProfile
        {
            public FacebookProfile()
            {
            }

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        /// <summary>
        /// Helpers implementing Facebook API calls.
        /// </summary>
        public static class FacebookHelpers
        {
            // The Facebook App Id
            public static readonly string FacebookAppId = Environment.GetEnvironmentVariable("OQPYFacebookAppId") ?? throw new NullReferenceException("OQPYFacebookAppId");

            // The Facebook App Secret
            public static readonly string FacebookAppSecret = Environment.GetEnvironmentVariable("OQPYFacebookAppSecret") ?? throw new NullReferenceException("OQPYFacebookAppSecret");

            public async static Task<FacebookAcessToken> ExchangeCodeForAccessToken(ConversationReference conversationReference, string code, string facebookOauthCallback)
            {
                var redirectUri = GetOAuthCallBack(conversationReference, facebookOauthCallback);
                var uri = GetUri("https://graph.facebook.com/v2.9/oauth/access_token",
                    ("client_id", FacebookAppId),
                    ("redirect_uri", redirectUri),
                    ("client_secret", FacebookAppSecret),
                    ("code", code)
                    );

                return await FacebookRequest<FacebookAcessToken>(uri);
            }

            public static async Task<bool> ValidateAccessToken(string accessToken)
            {
                var uri = GetUri("https://graph.facebook.com/debug_token",
                    ("input_token", accessToken),
                    ("access_token", $"{FacebookAppId}|{FacebookAppSecret}"));

                var res = await FacebookRequest<object>(uri).ConfigureAwait(false);
                return (((dynamic)res)?.data)?.is_valid;
            }

            public static async Task<string> GetFacebookProfileName(string accessToken)
            {
                var uri = GetUri("https://graph.facebook.com/v2.9/me",
                    ("fields", "id,name"),
                    ("access_token", accessToken));

                var res = await FacebookRequest<FacebookProfile>(uri);
                return res.Name;
            }

            private static string GetOAuthCallBack(ConversationReference conversationReference, string facebookOauthCallback)
            {
                var uri = GetUri(facebookOauthCallback,
                    ("userId", TokenEncoder(conversationReference.User.Id)),
                    ("botId", TokenEncoder(conversationReference.Bot.Id)),
                    ("conversationId", TokenEncoder(conversationReference.Conversation.Id)),
                    ("serviceUrl", TokenEncoder(conversationReference.ServiceUrl)),
                    ("channelId", conversationReference.ChannelId)
                    );
                return uri.ToString();
            }

            // because of a limitation on the characters in Facebook redirect_uri, we don't use the serialization of the cookie.
            // http://stackoverflow.com/questions/4386691/facebook-error-error-validating-verification-code
            public static string TokenEncoder(string token)
            {
                return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(token));
            }

            public static string TokenDecoder(string token)
            {
                return Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(token));
            }

            public static string GetFacebookLoginURL(ConversationReference conversationReference, string facebookOauthCallback)
            {
                var redirectUri = GetOAuthCallBack(conversationReference, facebookOauthCallback);
                var uri = GetUri("https://www.facebook.com/v2.9/dialog/oauth",
                    ("client_id", FacebookAppId),
                    ("redirect_uri", redirectUri),
                    ("response_type", "code"),
                    ("scope", "public_profile,email"),
                    ("state", Convert.ToString(new Random().Next(9999)))
                    );

                return uri.ToString();
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

            private static Uri GetUri(string endPoint, params (string, string)[] queryParams)
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                foreach (var queryparam in queryParams)
                {
                    queryString[queryparam.Item1] = queryparam.Item2;
                }

                var builder = new UriBuilder(endPoint)
                {
                    Query = queryString.ToString()
                };
                return builder.Uri;
            }
        }
    }
}