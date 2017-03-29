//#define DEBUG1
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.Dialogs;

//using Microsoft.Cognitive.LUIS;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using OQPYModels.Extensions;
using OQPYModels.Models.CoreModels;
using OQPYModels.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OQPYBot
{
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
        private readonly List<string> _propertyKeys = new List<string> { "name", "email", "location" };
        private readonly List<string> _cardActions1 = new List<string> { "Info", "Comments", "Reservate", "See list of reservations" };
        private readonly List<string> _cardActions2 = new List<string> { "yes", "no" };

        [LuisIntent("")]
        [LuisIntent("none")]
        public async Task None(IDialogContext context, LuisResult result)
        {
#if DEBUG1
            await DebugOut(context, "None");
#endif
            var message = context.MakeMessage();

            string mess = "Intent | score" + Environment.NewLine + "-------|-------" + Environment.NewLine +
                          (from _ in result.Intents
                           select $"{_.Intent} | {_?.Score?.ToString() ?? "0"}{Environment.NewLine}").Aggregate((a, b) => a + b);
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            context.UserData.TryGetValue(_name, out string name);
            telemetry.TrackTrace("Q",
                           SeverityLevel.Information,
                           new Dictionary<string, string> { { name ?? "Unnamed", result.Query + ":\r\n" + message.Text } });
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
#if DEBUG1
            await DebugOut(context, "SelfInfo");
#endif
            await ApplyProperty(context, result, _name);
        }

        [LuisIntent("self.info.email")]
        public async Task SelfInfoEmail(IDialogContext context, LuisResult result)
        {
#if DEBUG1
            await DebugOut(context, "SelfInfoEmail");
#endif
            await ApplyProperty(context, result, _email);
        }

        [LuisIntent("self.info.location")]
        public async Task SelfInfoLocation(IDialogContext context, LuisResult result)
        {
#if DEBUG1
            await DebugOut(context, "SelfInfoLocation");
#endif
            await ApplyProperty(context, result, _location);
        }

        [LuisIntent("self.info.get")]
        public async Task SelfInfoGet(IDialogContext context, LuisResult result)
        {
#if DEBUG1
            await DebugOut(context, "SelfInfoGet");
#endif
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
#if DEBUG1
            await DebugOut(context, "SelfInfoGet");
#endif
            var message = context.MakeMessage();
            message.Attachments = MakeACard(TestObjects.VenuesTest).ToList();
            message.AttachmentLayout = "carousel";

            await context.PostAsync(message);
        }

        public IEnumerable<Attachment> MakeACard(IEnumerable<BaseVenue> venue)
        {
            var comonTags = venue.IntersectionTags();
            var subtitle = comonTags.TagsToString((i) => $"{i.TagName} ");
            return from _ in venue
                   select new HeroCard(_.Name, subtitle, _.Tags.TagsToString((i) => $"{i.TagName} "), MakeImage(_), MakeCardActions().ToList()).ToAttachment();
        }

        public IEnumerable<CardImage> MakeListOfImages(IEnumerable<BaseVenue> venue)
        {
            var images = from _ in venue
                         select new CardImage(_.ImageUrl);
            return images;
        }

        public List<CardImage> MakeImage(BaseVenue venue)
        {
            return new List<CardImage>() { new CardImage(venue.ImageUrl) };
        }

        public IEnumerable<CardAction> MakeCardActions()
        {
            return from _ in _cardActions1
                   select new CardAction() { Title = _, Value = _, Type = "imBack" };
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
        public override async Task StartAsync(IDialogContext context)
        {
#if DEBUG1
            await DebugOut(context, "StartAsync");
#endif
            await base.StartAsync(context);
        }

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
#if DEBUG1
            await DebugOut(context, "MessageRecived");
#endif
            await base.MessageReceived(context, item);
        }

        private async Task DebugOut(IDialogContext context, string methode)
        {
            await context.PostAsync($"DEBUG from methode: {methode} ");
            await context.PostAsync($"frame:");
            await context.PostAsync((from _ in context.Frames
                                     select $"{_.Method.Name}").Aggregate((i, j) => i + Environment.NewLine + j));
            await context.PostAsync($"Private data={context.PrivateConversationData.Count}");
            await context.PostAsync($"Conversation data={context.ConversationData.Count}");
            await context.PostAsync($"User data={context.UserData.Count}");
        }
    }
}