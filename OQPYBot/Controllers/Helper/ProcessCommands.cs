using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OQPYClient.APIv03;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYBot.Controllers.Helper.Constants;
using static OQPYBot.Controllers.Helper.Helper;
using System;

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
            var totalResources = venue.Resources == null ? 0 : venue.Resources.Count;
            var freeResources = venue.Resources == null ? 0 : venue.Resources.Where(i => i.OQPYed == false).Count();
            var gotGeo = context.UserData.TryGetValue(_facebooklocation, out Geo geo);
            var distanceString = gotGeo && venue.Location != null ? $"{venue.Location.ToKilometers(new Location(geo.longitude, geo.latitude))} km" : "Can't get distane";
            var cards = new List<Attachment> {
                new ThumbnailCard(_locationObj, distanceString, venue?.Location?.Adress ?? venue?.Location?.ToString() ?? "Can't get location").ToAttachment(),
                new ThumbnailCard(_resourcesObj, null, $"{freeResources} free out of {totalResources}", null, MakeCardActions(venueId, _venueObj, _actionResources ).ToList()).ToAttachment(),
                new ThumbnailCard(_reservationsObj, $"There are {venue?.Reservations.Count() ?? 0} reservations", null, null, MakeCardActions(venueId, _venueObj, _actionReservations).ToList()).ToAttachment(),
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
                cards.AddRange(from _ in venue.Reservations.Take(9)
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
                                .ToAttachment());
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
                cards.AddRange(
                    from _ in venue.Resources.Take(9)
                    select new ThumbnailCard(_.StuffName, _.OQPYed ? "Taken :/" : "Free :)", _.GetInfo(), null, MakeCardActions(_.Id, _resourcesObj, _actionReservations).ToList()).ToAttachment()
                    );
            }
            return cards;
        }
        internal async static Task<IEnumerable<Attachment>> VenueComments(IDialogContext context, string venueId)
        {
            var venue = await _api.ApiVenuesSingleGetAsync(venueId);
            List<Attachment> cards = new List<Attachment>() {
            new ThumbnailCard(_commentsObj,
                venue.Reviews == null ? $"There aren't any comments for {venue.Name}" : $"There are {venue.Reviews.Count} Comments",
                $"You can add a review by clicking {_actionAdd} button",
                null,
                MakeCardActions(venueId, _commentsObj, _actionAdd).ToList()).ToAttachment()
            };
            if ( venue.Reviews != null )
            {
                cards.AddRange(from _ in venue.Reviews.Take(9)
                               select
                               new HeroCard(
                                   _commentsObj,
                                   $"{_.Rating}/10",
                                   _.Comment,
                                   null,
                                   MakeCardActions(
                                      _.Id,
                                      _commentsObj,
                                      _likeDislikeActions
                                      )
                                      .ToList())
                                .ToAttachment());
            }
            return cards;
        }

        internal async static Task<IEnumerable<Attachment>> CommensAdd(IDialogContext arg1, string CommentId)
        {
            throw new NotImplementedException();
        }
        internal static Task<IEnumerable<Attachment>> CommensDislike(IDialogContext arg1, string arg2) => throw new NotImplementedException();
        internal static Task<IEnumerable<Attachment>> CommensLike(IDialogContext arg1, string arg2) => throw new NotImplementedException();
        internal static Task<IEnumerable<Attachment>> CommentsRead(IDialogContext context, string CommentId)
        {
            throw new NotImplementedException();
        }
    }
}