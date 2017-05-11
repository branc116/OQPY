using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OQPYBot.Controllers.Helper.Constants;

namespace OQPYBot.Controllers.ProcessDialogs
{
    public static class ProcessDialogs
    {
        internal async static Task ProcessAddReview(IDialogContext context, IAwaitable<AddReview> result)
        {
            var review = await result;
            var gotVenue = context.ConversationData.TryGetValue(_tempVenueId, out string venueId);

            if ( !gotVenue )
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
    }
}