using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OQPYBot.Helper;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYBot.Helper.Constants;
using static OQPYBot.Helper.Helper;

namespace OQPYBot.Dialogs.ProcessDialogs
{
    public static class ProcessDialogs
    {
        private const string TAG = nameof(ProcessDialogs);

        internal async static Task ProcessAddReview(IDialogContext context, IAwaitable<AddReview> result)
        {
            var review = await result;
            var gotVenue = context.ConversationData.TryGetValue(_tempVenueId, out string venueId);

            if (!gotVenue)
            {
                await context.PostAsync("There was an error, please try again.");
                return;
            }
            var realReview = new Review(review.Grade, review.Review);

            await _api.ApiReviewsVenueReviewPostAsync(review.Review, venueId, review.Grade);
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>
            {
                new HeroCard(realReview.GetHelpfunessAndScore(), text: realReview.Comment.Length < 80 ? realReview.Comment : string.Empty).ToAttachment()
            };
            message.Text = realReview.Comment.Length > 80 ? realReview.Comment : string.Empty;
            await context.PostAsync("This review was added: ");
            await context.PostAsync(message);
            return;
        }

        internal static async Task ProcessVenues(IDialogContext context, SearchVenues result)
        {
            var like = result;
            Venue likeVenue;
            IEnumerable<Venue> venues;
            var haveLoc = context.UserData.TryGetValue(_facebooklocation, out Geo geolocation);
            if (haveLoc)
            {
                venues = await like.QAsync(likeVenue = new Venue()
                {
                    Name = like.Name == "n" ? null : like.Name,
                    Location = new Location(geolocation.longitude, geolocation.latitude)
                });
            }
            else
                venues = await like.QAsync(likeVenue = like.GetVenue());
            if (venues.Any())
            {
                var message = context.MakeMessage();

                context.ConversationData.SetValue(_currentActiveVenues, venues);
                Log.BasicLog("venues", venues.Select(i => i.ToString()).Aggregate((i, j) => $"{i}\n{j}"), SeverityLevel.Verbose);

                message.Attachments = MakeACard(venues, geolocation == null ? null : new Location(geolocation.longitude, geolocation.latitude)).ToList();
                message.AttachmentLayout = _layoutCarousel;
                await context.PostAsync(message);
            }
            else
            {
                Log.BasicLog(TAG, $"Venue not found, name: {likeVenue.Name}, locatio: {likeVenue?.Location.ToString() ?? null}", SeverityLevel.Error);
                await context.PostAsync("Sorry, can't find anything... :(");
            }
        }

        internal async static Task ProcessSignIn(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
        }

        internal async static Task ProcessReservation(IDialogContext context, IAwaitable<DateTimePicker> result)
        {
            var a = await result;
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (context.PrivateConversationData.TryGetValue(_tempResourceId, out string tempResource))
            {
                if (context.UserData.TryGetValue(_facebookToken, out string token))
                {
                    if (await FacebookHelper.FacebookHelpers.ValidateAccessToken(token))
                    {
                        await _api.ApiReservationsResourceReservationPostAsync(_tempResourceId, token, a.Time, a.Time.AddHours(a.Duration));
                        await context.PostAsync("Reservation added :)");
                    }
                }
                else
                {
                    var message = context.MakeMessage();
                    message.Attachments = ErrorAttachment("Not signed in", "Sign in to continue");
                    await context.PostAsync(message);
                    return;
                }
            }
            else
            {
                var message = context.MakeMessage();
                message.Attachments = ErrorAttachment("Ups", "Something went wrong...");
                await context.PostAsync(message);
                return;
            }
        }
    }
}