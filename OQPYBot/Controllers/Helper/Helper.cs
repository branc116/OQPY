using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static OQPYBot.Controllers.Helper.Constants;
using static OQPYModels.Extensions.Extensions;
namespace OQPYBot.Controllers.Helper
{
    public class Helper
    {
        public static event EventHandler<EventArgs> WaitContextPrompt;
        public static IEnumerable<Attachment> MakeACard(IEnumerable<Venue> venue)
        {
            
            var comonTags = venue.IntersectionTags();
            var subtitle = comonTags.TagsToString((i) => $"{i.TagName} ");
            return from _ in venue
                   select new HeroCard(_.Name, subtitle, _.Tags.TagsToString((i) => $"{i.TagName} "), MakeImage(_), MakeCardActions().ToList()).ToAttachment();
        }

        public static IEnumerable<CardImage> MakeListOfImages(IEnumerable<Venue> venue)
        {
            var images = from _ in venue
                         select new CardImage(_.ImageUrl);
            return images;
        }

        public static List<CardImage> MakeImage(Venue venue)
        {
            return new List<CardImage>() { new CardImage(venue.ImageUrl) };
        }

        public static IEnumerable<CardAction> MakeCardActions()
        {
            return from _ in _cardActions
                   select new CardAction() { Title = _, Value = _, Type = "imBack" };
        }

        public static async Task ApplyProperty(IDialogContext context, LuisResult result, params string[] propertyName)
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
                await context.PostAsync("I didn't get that :(");
                WaitContextPrompt?.Invoke(context, EventArgs.Empty);
            }
        }

        public static async Task ConfirmPropertyAboutUser(IDialogContext conx, IAwaitable<bool> args)
        {
            var res = await args;
            if (!conx.PrivateConversationData.TryGetValue(_propNames, out string[] propertyName))
            {
                WaitContextPrompt?.Invoke(conx, EventArgs.Empty);
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
            
            WaitContextPrompt?.Invoke(conx, EventArgs.Empty);
        }
    }
}