//#define DEBUG
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
//using Microsoft.Cognitive.LUIS;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using LuisBot.Services;
using Microsoft.ApplicationInsights.DataContracts;

namespace OQPYBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        BingSpellCheckService BingSpelling = new BingSpellCheckService();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                activity.Text = await BingSpelling.GetCorrectedTextAsync(activity.Text) ?? activity.Text;
                try
                {
                    await Conversation.SendAsync(activity, () => new LuisDialogOQPY());
                    
                }
                catch(Exception ex)
                {
                    var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
                    telemetry.TrackTrace("ExceptionInPost", SeverityLevel.Critical, new Dictionary<string, string> { { "Exceptions", ex.ToString() } });
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}