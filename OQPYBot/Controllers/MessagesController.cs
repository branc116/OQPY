using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
//using Microsoft.Cognitive.LUIS;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using LuisBot.Services;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.ApplicationInsights.DataContracts;

namespace OQPYBot1
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
                await Conversation.SendAsync(activity, () => new LuisDialogOQPY());
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
    [LuisModel("2f4d5a10-e2cf-4238-ab65-51ab4b4dd0ea", "b36329fcaa154546ba25f10bc5740770")]
    [Serializable]
    public class LuisDialogOQPY : LuisDialog<object>
    {
        private const string _propNames = "propertyNames";
        private const string _name = "name";
        private const string _email = "email";
        private const string _location = "location";
        private const string _sublocation = "SubLocation";
        private const string _subsublocation = "SubSubLocation";

        [LuisIntent("")]
        [LuisIntent("none")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();

            string mess = "Intent | score" + Environment.NewLine + "-------|-------" + Environment.NewLine +
                          (from _ in result.Intents
                           select $"{_.Intent} | {_?.Score?.ToString() ?? "0"}{Environment.NewLine}").Aggregate((a, b) => a + b);
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            context.UserData.TryGetValue(_name, out string name);
            telemetry.TrackTrace("Q",
                           SeverityLevel.Information,
                           new Dictionary<string, string> { { name, result.Query + ":\r\n" + message.Text } });
            message.Text = mess;
            await context.PostAsync($"Hi {name ?? ""}!");
            await Task.Delay(400);
            await context.PostAsync($"This: \"{result.Query}\" doesen't look like anything to me :(");
            await Task.Delay(400);
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("self.info")]
        public async Task SelfInfo(IDialogContext context, LuisResult result)
        {
            await ApplyProperty(context, result, _name);
        }
        [LuisIntent("self.info.email")]
        public async Task SelfInfoEmail(IDialogContext context, LuisResult result)
        {
            await ApplyProperty(context, result, _email);
        }
        [LuisIntent("self.info.location")]
        public async Task SelfInfoLocation(IDialogContext context, LuisResult result)
        {
            await ApplyProperty(context, result, _location);
        }
        private async Task ApplyProperty(IDialogContext context, LuisResult result, params string[] propertyName)
        {
            for (int i = 0; i < result.Entities.Count; i++)
            {
                result.Entities[i].Type = result.Entities[i].Type.Replace("builtin.", string.Empty);
            }
            var items = from _ in result.Entities
                        let type = _.Type
                        where propertyName?.Contains(type) ?? false
                        select new { Key = type, Value = _.Entity };

            if (items.Any())
            {
                var question = (from _ in items
                                select $"Is your {_.Key} {_.Value}?")
                                .Aggregate((i, j) => $"{i.Replace('?', ' ')} and {j.ToLower()}");
                foreach (var prop in items)
                    context.PrivateConversationData.SetValue(prop.Key, prop.Value);
                context.PrivateConversationData.SetValue(_propNames, propertyName);
                PromptDialog.Confirm(context, ConfirmPropertyAboutUser, question);
            }
            else
            {
                await None(context, result);
            }
        }
        private async Task ConfirmPropertyAboutUser(IDialogContext conx, IAwaitable<bool> args)
        {
            var res = await args;
            if (!conx.PrivateConversationData.TryGetValue(_propNames, out string[] propertyName))
            {
                conx.Wait(this.MessageReceived);
                return;
            }
            foreach (var pro in propertyName)
            {
                if (conx.PrivateConversationData.TryGetValue(pro, out string prop))
                {

                    if (res)
                    {
                        conx.UserData.SetValue(pro, prop);
                        await conx.PostAsync($"Ok, your {pro} is now {prop}");
                    }
                    else
                    {
                        await conx.PostAsync($"Then what is your {pro}?");
                    }
                }
                else
                {
                    await conx.PostAsync($"Something went wrong, ups :/");
                }
            }
            conx.Wait(this.MessageReceived);
        }
        //private async Task ConfirmPropertyChange(IDialogContext context, IAwaitable<bool> result)
        //{
        //    var rez = await result;
        //    if (context.PrivateConversationData.TryGetValue("propertyName", out string propertyName))
        //    {
        //        context.Wait(this.MessageReceived);
        //        return;
        //    }
        //    context.UserData.TryGetValue(propertyName, out string test);
        //    context.PrivateConversationData.TryGetValue(propertyName, out string name);
        //    if (rez)
        //    {
        //        context.UserData.SetValue(propertyName, name);
        //        await context.PostAsync($"Ok, your {propertyName} is now {name}");
        //    }
        //    else
        //        await context.PostAsync($"Ok, your {propertyName} will stay {test}");
        //    context.Wait(this.MessageReceived);
        //}
    }

}