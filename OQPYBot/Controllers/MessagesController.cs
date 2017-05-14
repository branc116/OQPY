//#define DEBUG
using LuisBot.Services;
using Microsoft.ApplicationInsights.DataContracts;

//using Microsoft.Cognitive.LUIS;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Rest.Serialization;
using OQPYBot.Dialogs;
using OQPYBot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OQPYBot
{
    [BotAuthentication]
    public class MessagesController: ApiController
    {
        private BingSpellCheckService BingSpelling = new BingSpellCheckService();

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                //Record(activity);
                if ( activity.Type == ActivityTypes.Message )
                {

                    activity.Text = await BingSpelling.GetCorrectedTextAsync(activity.Text) ?? activity.Text ?? "none";
                    await Conversation.SendAsync(activity, () => new LuisDialogOQPY());
                }
                else
                {
                    await HandleSystemMessageAsync(activity);
                }
            }
            catch ( Exception ex )
            {
                var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
                telemetry.TrackTrace("ExceptionInPost", SeverityLevel.Critical, new Dictionary<string, string> { { "Exceptions", ex.ToString() } });
            }
            var response = Request?.CreateResponse(HttpStatusCode.OK) ?? null;
            return response;
        }

        private static object objectLock = new object();

        private static void Record(Activity act)
        {
            lock ( objectLock )
            {
                var delimiter = "^^ˇˇ\n";
                var pathOut = @"C:\Users\Branimir\ActivityLog.log";
                if ( !File.Exists(pathOut) )
                    File.Create(pathOut);
                var newJson = SafeJsonConvert.SerializeObject(act, Constants._safeDeserializationSettings);
                newJson = delimiter + newJson + delimiter;
                File.AppendAllText(pathOut, newJson);
            }
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity activity)
        {
            if ( activity.Type == ActivityTypes.DeleteUserData )
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if ( activity.Type == ActivityTypes.ConversationUpdate )
            {
                //IConversationUpdateActivity update = activity;
                //using ( var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity) )
                //{
                //    var client = scope.Resolve<IConnectorClient>();
                //    if ( update.MembersAdded.Any() )
                //    {
                //        var reply = activity.CreateReply();
                //        foreach ( var newMember in update.MembersAdded )
                //        {
                //            if ( newMember.Id != activity.Recipient.Id )
                //            {
                //                //reply.Text = $"Welcome {newMember.Name}!";
                //            }
                //            else
                //            {
                //                //reply.Text = $"Welcome {activity.From.Name}";
                //            }
                //            //await client.Conversations.ReplyToActivityAsync(reply);
                //        }
                //    }
                //}
            }
            else if ( activity.Type == ActivityTypes.ContactRelationUpdate )
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if ( activity.Type == ActivityTypes.Typing )
            {
                // Handle knowing tha the user is typing
            }
            else if ( activity.Type == ActivityTypes.Ping )
            {
            }

            return null;
        }
    }
}