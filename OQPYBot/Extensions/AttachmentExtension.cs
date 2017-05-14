using Microsoft.Bot.Connector;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYBot.Helper.Constants;
using static OQPYBot.Helper.Helper;

namespace OQPYBot.Extensions
{
    public static class ReviewAttachmentExtension
    {
        public static Attachment ToAttachment(this Review comment)
        {
            return new HeroCard(comment.GetHelpfunessAndScore(), null, comment.Comment.Length < 80 ? comment.Comment : string.Empty, buttons: comment.ToCardActions()).ToAttachment();
        }

        public static IEnumerable<Attachment> ToAttachments(this IEnumerable<Review> comments)
        {
            return from _ in comments.Take(9)
                   select
                   new HeroCard(
                       _.GetHelpfunessAndScore(),
                       null,
                       _.Comment,
                       null,
                       MakeCardActions(
                          _.Id,
                          _commentsObj,
                          _actionLike, _actionDislike, _actionReadFull
                          )
                          .ToList())
                    .ToAttachment();
        }

        public static IList<CardAction> ToCardActions(this Review comment, params string[] additionActions)
        {
            List<string> actions;
            if ( additionActions != null && additionActions.Length > 0 )
            {
                actions = new List<string>(_commentsCardActions);
                actions.AddRange(additionActions);
                return MakeCardActions(comment.Id, _commentsObj, actions).ToList();
            }
            return MakeCardActions(comment.Id, _commentsObj, _likeDislikeActions).ToList();
        }

        public static Attachment AddNew(Venue venue)
        {
            return new ThumbnailCard(_commentsObj,
                venue.Reviews == null ? $"There aren't any comments for {venue.Name}" : $"There are {venue.Reviews.Count} Comments",
                $"You can add a review by clicking {_actionAdd} button",
                null,
                MakeCardActions(venue.Id, _commentsObj, _actionAdd).ToList()).ToAttachment();
        }
    }

    public static class VenueAttachmentExtension
    {
        public static Attachment ToLocationAttachment(this Venue venue, Geo geo)
        {
            var distanceString = geo != null && venue.Location != null ? $"{venue.Location.ToKilometers(new Location(geo.longitude, geo.latitude))} km" : "Can't get distane";
            return new ThumbnailCard(_locationObj, distanceString, venue?.Location?.Adress ?? venue?.Location?.ToString() ?? "Can't get location").ToAttachment();
        }

        public static Attachment ToResourcesAttachment(this Venue venue)
        {
            var totalResources = venue.Resources == null ? 0 : venue.Resources.Count;
            var freeResources = venue.Resources == null ? 0 : venue.Resources.Where(i => i.OQPYed == false).Count();
            return new ThumbnailCard(_resourcesObj, null, $"{freeResources} free out of {totalResources}", null, MakeCardActions(venue.Id, _venueObj, _actionResources).ToList()).ToAttachment();
        }

        public static Attachment ToReservationsAttachment(this Venue venue, params string[] actions)
        {
            return new ThumbnailCard(_reservationsObj, $"There are {venue?.Reservations.Count() ?? 0} reservations", null, null, MakeCardActions(venue.Id, _venueObj, actions).ToList()).ToAttachment();
        }

        public static Attachment ToCommentsAttachment(this Venue venue)
        {
            return new ThumbnailCard(_commentsObj, $"There are {venue?.Reviews.Count() ?? 0} comments", null, null, MakeCardActions(venue.Id, _venueObj, _actionComments).ToList()).ToAttachment();
        }
    }

    public static class ReservationsAttachmentExtension
    {
        public static IEnumerable<Attachment> ToAttachments(this IEnumerable<Reservation> reservations)
        {
            return from _ in reservations.Take(9)
                   select
                   new ThumbnailCard(
                       _reservationsObj,
                       _.Resource.StuffName,
                       $"From ${_.StartReservationTime} to ${_.EndReservationTime}",
                       null,
                       MakeCardActions(
                          _.Id,
                          _reservationsObj,
                          _reservationsCardActions)
                          .ToList())
                    .ToAttachment();
        }
    }

    public static class ResourcesAttachmentExtension
    {
        public static IEnumerable<Attachment> ToAttachments(this IEnumerable<Resource> resources)
        {
            return from _ in resources.Take(9)
                   select new ThumbnailCard(_.StuffName, _.OQPYed ? "Taken :/" : "Free :)", _.GetInfo(), null, MakeCardActions(_.Id, _resourcesObj, _actionReservations).ToList()).ToAttachment();
        }

        public static IEnumerable<Attachment> ToAttachments<T>(this IEnumerable<T> items, Func<T, string> title, Func<T, string> subTitle, Func<T, string> text, string Obj, Func<T, string> ObjId, params string[] actions) where T : class
        {
            return from _ in items.Take(9)
                   select new ThumbnailCard(title(_), subTitle(_), text(_), null, MakeCardActions(ObjId(_), Obj, actions).ToList()).ToAttachment();
        }
    }

    public static class CommentsAttachmentExtension
    {
    }
}