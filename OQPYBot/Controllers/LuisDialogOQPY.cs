//#define DEBUG1
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Microsoft.Rest.Serialization;
using OQPYBot.Helper;
using OQPYModels.Models.CoreModels;
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
                    if ( context.PrivateConversationData.TryGetValue(_insideDialogKey, out bool inside) && inside )
                    {
                        context.Wait(MessageReceived);
                        context.PrivateConversationData.SetValue(_insideDialogKey, false);
                    }
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
                             where context.UserData.ContainsKey(_)
                             select $"{_}|{context.UserData.Get<object>(_)}")
                            .Aggregate((i, j) => $"{i}{Environment.NewLine}{j}");
            await context.PostAsync(message);
        }

        [LuisIntent("venue.search")]
        public async Task VenueSearch(IDialogContext context, LuisResult result)
        {
            await DebugOut(context, "SelfInfoGet");
            var search = LuisDialogSearchVenues.Create(context);
            if ( context.ConversationData.TryGetValue(_channelId, out string chid) && chid == "facebook" )
                await context.PostAsync("It'd be nice if you'd send the location of where you wanna search :)");
            //context.Call(search, ProcessVenues);
            var message = context.MakeMessage();
            await context.Forward(search, ProcessVenues, message, default(System.Threading.CancellationToken));

        }

        private async Task ProcessVenues(IDialogContext context, IAwaitable<SearchVenues> result)
        {
            var like = (await result);
            IEnumerable<Venue> venues;
            if ( context.ConversationData.TryGetValue(_channelId, out string chid) && chid == "facebook" && context.UserData.TryGetValue(_facebooklocation, out Geo geolocation) )
                venues = await like.QAsync(geolocation);
            else
                venues = await like.QAsync();
            if ( venues.Any() )
            {
                var message = context.MakeMessage();

                context.ConversationData.SetValue(_currentActiveVenues, venues);
                Log.BasicLog("venues", venues.Select(i => i.ToString()).Aggregate((i, j) => $"{i}\n{j}"), SeverityLevel.Verbose);

                message.Attachments = MakeACard(venues).ToList();
                message.AttachmentLayout = "carousel";
                await context.PostAsync(message);
            }else
            {
                await context.PostAsync("Sorry, can't find anything... :(");
            }
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
            context.ConversationData.SetValue(_channelId, a.ChannelId);
            if ( a.ChannelId == "facebook" )
            {
                if (a.Entities != null && a.Entities.Count > 0 )
                {
                    try
                    {
                        var loc = a.Entities[0].GetAs<FacebookLocation>();
                        context.UserData.SetValue(_facebooklocation, loc.geo);
                        await context.PostAsync("Got it! :)");
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    catch ( Exception ex )
                    {

                    }
                }
                //var reply = context.MakeMessage();
                //reply = GimmeLocationFacebook(reply);
                //await context.PostAsync(reply);
            }
            await base.MessageReceived(context, item);
            Log.BasicLog(a);
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