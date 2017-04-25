//#define DEBUG1
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using OQPYBot.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static OQPYBot.Controllers.Helper.Constants;
using static OQPYBot.Controllers.Helper.Helper;

namespace OQPYBot.Controllers
{
    [LuisModel("2f4d5a10-e2cf-4238-ab65-51ab4b4dd0ea", "b36329fcaa154546ba25f10bc5740770")]
    [Serializable]
    public class LuisDialogOQPY: LuisDialog<object>
    {
        public LuisDialogOQPY()
        {
            WaitContextPrompt += (conx, args) =>
            {
                if ( conx is IDialogContext context )
                {
                    context.Wait(MessageReceived);
                }
            };
        }

        [LuisIntent("")]
        [LuisIntent("none")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "None");
            var message = context.MakeMessage();

            string mess = "Intent | score" + Environment.NewLine + "-------|-------" + Environment.NewLine +
                          (from _ in result?.Intents
                           select $"{_.Intent} | {_?.Score?.ToString() ?? "0"}{Environment.NewLine}").Aggregate((a, b) => a + b) ?? "No intents";

            context.UserData.TryGetValue(_name, out string name);
            Log.BasicLog(name ?? "Unnamed", result.Query + "\r\n" + mess, SeverityLevel.Information);
            message.Text = mess;
            await context.PostAsync($"Hi {name ?? ""}!");
            await context.PostAsync($"This: \"{result.Query}\" doesn't look like anything to me :(");
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("self.info")]
        public async Task SelfInfo(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfo");
            await ApplyProperty(context, result, _name);
        }

        [LuisIntent("self.info.email")]
        public async Task SelfInfoEmail(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfoEmail");
            await ApplyProperty(context, result, _email);
        }

        [LuisIntent("self.info.location")]
        public async Task SelfInfoLocation(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfoLocation");
            await ApplyProperty(context, result, _location);
        }

        [LuisIntent("self.info.get")]
        public async Task SelfInfoGet(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfoGet");
            var message = context.MakeMessage();
            message.Text = $"Property | Value{Environment.NewLine}---|---{Environment.NewLine}";
            message.Text += (from _ in _propertyKeys
                             where context.UserData.ContainsKey(_) && context.UserData.Get<object>(_).GetType() == typeof(string)
                             select $"{_}|{context.UserData.Get<string>(_)}").Aggregate((i, j) => $"{i}{Environment.NewLine}{j}");
            await context.PostAsync(message);
        }

        [LuisIntent("venue.search")]
        public async Task VenueSearch(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfoGet");

            context.Call(LuisDialogSearchVenues.Create(context), ProcessVenues);
        }

        private async Task ProcessVenues(IDialogContext context, IAwaitable<SearchVenues> result)
        {
            var message = context.MakeMessage();
            var like = (await result);
            message.Attachments = MakeACard(await like.QAsync()).ToList();
            message.AttachmentLayout = "carousel";
            await context.PostAsync(message);
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await DebugOut(context, "StartAsync");
            await base.StartAsync(context);
        }

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var a = await item;
            await DebugOut(context, "MessageRecived");
            if ( a.ChannelId == "facebook" )
            {
                var reply = context.MakeMessage();
                reply.ChannelData = new FacebookMessage
                (
                    text: "Please share your location with me.",
                    quickReplies: new List<FacebookQuickReply>
                    {
                        new FacebookQuickReply(
                            contentType: FacebookQuickReply.ContentTypes.Location,
                            title: default(string),
                            payload: default(string)
                        )
                    }
                );
            }
            await base.MessageReceived(context, item);
            Log.BasicLog("Attachments",
                    (a.Attachments != null && a.Attachments.Any()) ?
                        (from _ in a.Attachments
                         select _.Content).Aggregate((i, j) => $"{i}\n{j}") :
                         "No Attachments",
                    SeverityLevel.Information);
        }

        public async Task ExposeMessageRecived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await MessageReceived(context, item);
        }

        private async Task DebugOut(IDialogContext context, string methode)
        {
#if DEBUG1
            await context.PostAsync($"DEBUG from methode: {methode} ");
            await context.PostAsync($"frame:");
            await context.PostAsync((from _ in context.Frames
                                     select $"{_.Method.Name}").Aggregate((i, j) => i + Environment.NewLine + j));
            await context.PostAsync($"Private data={context.PrivateConversationData.Count}");
            await context.PostAsync($"Conversation data={context.ConversationData.Count}");
            await context.PostAsync($"User data={context.UserData.Count}");
#endif
        }
    }
}