//#define DEBUG1
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
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
    [Serializable]
    public class LuisDialogOQPY: LuisDialog<object>
    {
        private const String TAG = "DialogOQPY";

        public LuisDialogOQPY() : base(new LuisService(new LuisModelAttribute(Environment.GetEnvironmentVariable("OQPYLuisModelID"), Environment.GetEnvironmentVariable("OQPYLuisSubKey"))))
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
            var times = context.UserData.TryGetValue(_timesUsed, out int n);
            if ( n < 10 )
                await context.PostAsync($"I see you are new to OQPY, what you just entered made me search for {result.Query}, if you didn't wanted to do that and are lost, just type help, and I'll help");
            await Helper.Helper.ProcessVenues(context, new SearchVenues() { Name = result.Query });
            context.Wait(MessageReceived);
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
        public async Task VenueSearch(IDialogContext context, IAwaitable<IMessageActivity> item, LuisResult result)
        {
            await DebugOut(context, "SelfInfoGet");
            var search = LuisDialogSearchVenues.Create(context);
            var messagee = context.MakeMessage();
            await context.Forward(search, ProcessVenues, messagee, default(System.Threading.CancellationToken));
        }

        [LuisIntent("hello")]
        public async Task Hello(IDialogContext context, IAwaitable<IMessageActivity> item, LuisResult result)
        {
            await context.PostAsync(_hello[new Random().Next(0, _hello.Count)]);
        }

        [LuisIntent("help")]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> item, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Attachments = (await _processCommands[_commandBaseHelp](context, string.Empty)).ToList();
            message.AttachmentLayout = _layoutCarousel;
            await context.PostAsync(message);
        }

        private async Task GotLocaion(IDialogContext context, IAwaitable<dynamic> result)
        {
            Place p = await result;
        }

        private async Task ProcessVenues(IDialogContext context, IAwaitable<SearchVenues> result)
        {
            var venue = await result;
            await Helper.Helper.ProcessVenues(context, venue);
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
            if ( a.Text == "How much available spots in caffe bar History?" || a.Text == "Available spots in caffe bar History?" ||
                a.Text == "How much free spots in caffe bar History?" || a.Text == "Free spots in caffe bar History?" )
            {
                await context.PostAsync("3 free spots.");
                return;
            }
            if (a.Text.Substring(0,4).ToLower() == "oqpy")
            {
                await Help(context, item, null);
                return;
            }
            if ( context.UserData.TryGetValue(_timesUsed, out int times) )
            {
                context.UserData.SetValue(_timesUsed, times++);
            }
            else
            {
                context.UserData.SetValue(_timesUsed, 1);
            }
            if ( a.ChannelId == "facebook" )
            {
                if ( a.Entities != null && a.Entities.Count > 0 )
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
            else if ( a.Text == _commandBaseShareLocation )
            {
                await context.PostAsync("Sorry this isn't available on your platform :/");
            }
            if ( a.Text.StartsWith("||||") )
                await ProcessPostback(context, a);
            else
                await base.MessageReceived(context, item);
            Log.BasicLog(a);
        }

        private async Task ProcessPostback(IDialogContext context, IMessageActivity item)
        {
            var intent = item.Text.Split(new string[2] { "||||", ":" }, StringSplitOptions.RemoveEmptyEntries);
            var message = context.MakeMessage();
            bool skip = false;
            if ( _processCommands.ContainsKey(intent[0]) )
            {
                var attachments = await _processCommands[intent[0]](context, intent[1]);
                message.AttachmentLayout = "carousel";
                message.Attachments = attachments?.ToList();
                if ( message.Attachments == null )
                    skip = true;
            }
            else if ( intent[0] == _commandBaseShareLocation )
            {
                message.ChannelData = new FacebookMessage
                (
                    text: "Press the button to share location",
                    quickReplies: new List<FacebookQuickReply>
                    {
                        new FacebookQuickReply(
                            FacebookQuickReply.ContentTypes.Location,
                            default(string),
                            default(string)
                        )
                    }
                );
            }
            else
            {
                Log.BasicLog(TAG, $"Command not found, command: {item}", SeverityLevel.Error);
                message.Text = "Sorry that command isn't supported yet...";
            }
            if ( !skip )
                await context.PostAsync(message);
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