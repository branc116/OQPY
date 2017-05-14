using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OQPYBot.Dialogs;
using OQPYBot.Extensions;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OQPYBot.Dialogs.ProcessDialogs.ProcessDialogs;
using static OQPYBot.Helper.Constants;
using static OQPYBot.Helper.Helper;

namespace OQPYBot.Helper
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

        internal async static Task<IEnumerable<Attachment>> BaseShareLocation(IDialogContext arg1, string arg2)
        {
            return ErrorAttachment("Ups", "Not supported on your platform");
        }

        internal async static Task<IEnumerable<Attachment>> BaseSignIn(IDialogContext context, string arg2)
        {
            var mess = context.MakeMessage();
            mess.Text = "login";
            await context.Forward(SimpleFacebookAuthDialog.dialog, ProcessSignIn, mess, default(CancellationToken));
            return null;
        }

        internal async static Task<IEnumerable<Attachment>> BaseReservation(IDialogContext context, string arg2)
        {
            if (context.UserData.TryGetValue(_facebookToken, out string token))
            {
                var reservation = await _api.ApiReservationsMyGetAsync(token);
                if (reservation == null)
                    throw new ArgumentNullException(nameof(reservation));
                return reservation.ToAttachments(
                    title: i => i.StartReservationTime.ToShortTimeString(),
                    subTitle: i => $"{i.Duration.Hours}h",
                    text: i => i.Resource.StuffName,
                    Obj: _reservationsObj,
                    ObjId: i => i.Id,
                    actions: new string[] { _actionInfo, _actionDelete });
            }
            else
            {
                return ErrorAttachment("Not signed id", "Sign in pls");
            }
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
                venue.Reservations == null ? $"There aren't any reservations in {venue.Name}" : $"There are {venue.Reservations.Count()} reservations",
                $"You can add a reservaion by clicking {_actionAdd} button",
                null,
                MakeCardActions(venueId, _reservationsObj, _actionAdd).ToList()).ToAttachment()
            };
            if (venue.Reservations != null)
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
            if (venue.Resources != null)
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
            if (venue.Reviews != null)
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
            await context.Forward(dialog, ProcessAddReview, message, default(System.Threading.CancellationToken));
            return null;
        }

        internal async static Task<IEnumerable<Attachment>> CommensDislike(IDialogContext context, string commentId)
        {
            await _api.ApiReviewsLikeGetAsync(commentId, "0");
            return await CommentsRead(context, commentId);
        }

        internal static Task<IEnumerable<Attachment>> ReservationDelete(IDialogContext arg1, string arg2) => throw new NotImplementedException();

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
            if (comment.Comment.Length > 80)
            {
                await context.PostAsync(comment.Comment);
            }
            var attachments = new List<Attachment>
            {
                comment.ToAttachment()
            };

            return attachments;
        }

        internal async static Task<IEnumerable<Attachment>> ReservationAdd(IDialogContext context, string resourceId)
        {
            if (context.UserData.TryGetValue(_facebookToken, out string token))
            {
                if (await FacebookHelper.FacebookHelpers.ValidateAccessToken(token))
                {
                    var message = context.MakeMessage();
                    context.PrivateConversationData.SetValue(_tempResourceId, resourceId);
                    await context.Forward(DateTimePickerDialog.Create(context), ProcessReservation, message, default(CancellationToken));
                    return null;
                }
                else
                {
                    return ErrorAttachment("Sign in timed out", "It looks like you sign in timed out. Sign in again to continue.");
                }
            }
            else
            {
                return ErrorAttachment("Not signed in", "Sign in to make a reservation.");
            }
        }
    }
}