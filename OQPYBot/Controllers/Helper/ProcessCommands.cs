using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OQPYBot.Controllers.Extensions;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYBot.Controllers.Helper.Constants;
using static OQPYBot.Controllers.Helper.Helper;

namespace OQPYBot.Controllers.Helper
{
    public class ProcessCommands
    {
        internal async static Task<IEnumerable<Attachment>> BaseHelp(IDialogContext context, string arg2)
        {
            var attachemts = new List<Attachment>
            {
                new HeroCard("OQPY", "OQPY is a bot that helps you with your venue chices").ToAttachment(),
                new HeroCard("Search venues", "Search venues by just typing the name of the venue you wanna search", null, null, MakeCardActions("arg2", _baseObj, _actionNeatby).ToList()).ToAttachment(),
                new HeroCard("Share you location", "Make your searches more precise and select where you wanna search", null, null, MakeCardActions("arg2", _baseObj, _actionShareLocation).ToList()).ToAttachment(),
                new HeroCard("Sign in", "Sign in so that you can make reservations", null, null, MakeCardActions("arg2", _baseObj, _actionSignIn).ToList()).ToAttachment(),
            };
            return attachemts;
        }

        internal async static Task<IEnumerable<Attachment>> BaseSearchNearby(IDialogContext context, string arg2)
        {
            await ProcessVenues(context, new SearchVenues());
            return null;
        }

        public async static Task<IEnumerable<Attachment>> VenueInfo(IDialogContext context, string venueId)
        {
            var venue = await _api.ApiVenuesSingleGetAsync(venueId);
            var gotGeo = context.UserData.TryGetValue(_facebooklocation, out Geo geo);

            var cards = new List<Attachment> {
                venue.ToLocationAttachment(geo),
                venue.ToResourcesAttachment(),
                venue.ToReservationsAttachment(_actionReservations),
                new ThumbnailCard(_commentsObj, $"There are {venue?.Reviews.Count() ?? 0} comments", null, null, MakeCardActions(venueId, _venueObj, _actionComments).ToList()).ToAttachment(),
            };
            return cards;
        }

        internal async static Task<IEnumerable<Attachment>> VenueReservations(IDialogContext context, string venueId)
        {
            var venue = await _api.ApiVenuesSingleGetAsync(venueId);
            List<Attachment> cards = new List<Attachment>() {
            new ThumbnailCard(_reservationsObj,
                venue.Reservations == null ? $"There aren't any reservations in {venue.Name}" : $"There are {venue.Reviews.Count} Comments",
                $"You can add a reservaion by clicking {_actionAdd} button",
                null,
                MakeCardActions(venueId, _reservationsObj, _actionAdd).ToList()).ToAttachment()
            };
            if ( venue.Reservations != null )
            {
                cards.AddRange(venue.Reservations.ToAttachments());
            }
            return cards;
        }

        internal async static Task<IEnumerable<Attachment>> VenueResources(IDialogContext context, string venueId)
        {
            var venue = await _api.ApiVenuesSingleGetAsync(venueId);
            venue.FixLoops();
            var totalResources = venue.Resources == null ? 0 : venue.Resources.Count;
            var freeResources = venue.Resources == null ? 0 : venue.Resources.Where(i => i.OQPYed == false).Count();
            var cards = new List<Attachment>
            {
                new ThumbnailCard(_resourcesObj, null, $"{freeResources} free out of {totalResources}").ToAttachment()
            };
            if ( venue.Resources != null )
            {
                cards.AddRange(venue.Resources.ToAttachments());
            }
            return cards;
        }

        internal async static Task<IEnumerable<Attachment>> VenueComments(IDialogContext context, string venueId)
        {
            var venue = await _api.ApiVenuesSingleGetAsync(venueId);
            List<Attachment> cards = new List<Attachment>() {
                ReviewAttachmentExtension.AddNew(venue)
            };
            if ( venue.Reviews != null )
            {
                cards.AddRange(venue.Reviews.ToAttachments());
            }
            return cards;
        }

        internal async static Task<IEnumerable<Attachment>> CommensAdd(IDialogContext context, string venueId)
        {
            var dialog = DialogAddReview.Create(context);
            context.ConversationData.SetValue(_tempVenueId, venueId);
            var message = context.MakeMessage();
            await context.Forward(dialog, ProcessDialogs.ProcessDialogs.ProcessAddReview, message, default(System.Threading.CancellationToken));
            return null;
        }

        internal async static Task<IEnumerable<Attachment>> CommensDislike(IDialogContext context, string commentId)
        {
            await _api.ApiReviewsLikeGetAsync(commentId, "0");
            return await CommentsRead(context, commentId);
        }

        internal async static Task<IEnumerable<Attachment>> CommentsLike(IDialogContext context, string commentId)
        {
            await _api.ApiReviewsLikeGetAsync(commentId, "1");
            return await CommentsRead(context, commentId);
        }

        internal async static Task<IEnumerable<Attachment>> CommentsRead(IDialogContext context, string commentId)
        {
            var comment = await _api.ApiReviewsGetAsync(commentId);
            //await context.PostAsync(comment.Comment);
            var att = comment.ToAttachment();
            if ( comment.Comment.Length > 80 )
            {
                await context.PostAsync(comment.Comment);
            }
            var attachments = new List<Attachment>
            {
                comment.ToAttachment()
            };

            return attachments;
        }
    }
}