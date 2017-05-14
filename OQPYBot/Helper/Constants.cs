#define forceProd

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using OQPYClient.APIv03;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OQPYBot.Helper
{
    public static class Constants
    {
        public const string _managerUri =
#if DEBUG && !forceProd
                "http://localhost:5000/";
#else
                "https://oqpymanager.azurewebsites.net/";

#endif
        internal const string _imageError = "https://materialdesignicons.com/api/download/icon/svg/9BC1A338-CD03-4D39-BE46-E9DE5EE51A2F";

        internal const string _propNames = "propertyNames";
        internal const string _name = "name";
        internal const string _email = "email";
        internal const string _location = "location";
        internal const string _timesUsed = "timesUsed";
        internal const string _facebooklocation = "Facebook Location";
        internal const string _subsublocation = "SubSubLocation";
        internal const string _facebookToken = "facebookToken";

        internal const string _currentActiveVenues = "currentActiveVenues";

        internal const string _insideDialogKey = "insideProperty";

        internal const string _tempVenueId = "tempVenueId";
        internal const string _tempResourceId = "tempResourceId";
        internal const string _channelId = "chId";
        internal const string _layoutCarousel = "carousel";

        internal const string _actionInfo = "Info";
        internal const string _actionComments = "Comments";
        internal const string _actionReservations = "Reservations";
        internal const string _actionResources = "Resources";
        internal const string _actionAdd = "Add New";
        internal const string _actionLike = "Like";
        internal const string _actionDislike = "Dislike";
        internal const string _actionReservateAfter = "Reservate after";
        internal const string _actionReservateBefore = "reservate before";
        internal const string _actionReadFull = "Read";
        internal const string _actionNeatby = "Nearby";
        internal const string _actionHelp = "Help";
        internal const string _actionShareLocation = "Share location";
        internal const string _actionSignIn = "Sign in";
        internal const string _actionDelete = "delete";

        internal const string _baseObj = "Base";
        internal const string _venueObj = "Venue";
        internal const string _resourcesObj = "Resources";
        internal const string _commentsObj = "Comments";
        internal const string _reservationsObj = "Reservation";
        internal const string _locationObj = "Location";

        internal const string _commandBaseHelp = _actionHelp + _baseObj;
        internal const string _commandBaseNearby = _actionNeatby + _baseObj;
        internal const string _commandBaseShareLocation = _actionShareLocation + _baseObj;
        internal const string _commandBaseSignIn = _actionSignIn + _baseObj;
        internal const string _commandBaseReservations = _actionReservations + _baseObj;

        internal const string _commandVenueInfo = _actionInfo + _venueObj;
        internal const string _commandVenueComments = _actionComments + _venueObj;
        internal const string _commandVenueReservations = _actionReservations + _venueObj;
        internal const string _commandVenueResources = _actionResources + _venueObj;
        internal const string _commandVenueLike = _actionLike + _venueObj;
        internal const string _commandVenueDislike = _actionDislike + _venueObj;

        internal const string _commandResourcesInfo = _actionInfo + _resourcesObj;
        internal const string _commandResourcesRead = _actionReadFull + _resourcesObj;

        internal const string _commandCommentsAdd = _actionAdd + _commentsObj;
        internal const string _commandCommentsLike = _actionLike + _commentsObj;
        internal const string _commandCommentsDislike = _actionDislike + _commentsObj;
        internal const string _commandCommentsRead = _actionReadFull + _commentsObj;

        internal const string _commandReservateAfter = _actionReservateAfter + _reservationsObj;
        internal const string _commandReservateBefore = _actionReservateBefore + _reservationsObj;
        internal const string _commandReservateAdd = _actionAdd + _reservationsObj;
        internal const string _commandReservateDelete = _actionAdd + _actionDelete;

        internal static readonly List<string> _propertyKeys = new List<string> { _name, _email, _location, _facebooklocation, _currentActiveVenues };

        internal static readonly List<string> _likeDislikeActions = new List<string> { _actionLike, _actionDislike };
        internal static readonly List<string> _venueCardActions = new List<string> { _actionInfo, _actionComments, _actionReservations };
        internal static readonly List<string> _resourcesCardActions = new List<string> { _actionInfo };
        internal static readonly List<string> _commentsCardActions = new List<string> { _actionAdd };
        internal static readonly List<string> _reservationsCardActions = new List<string> { _commandReservateBefore, _commandReservateAfter };
        internal static readonly List<string> _hello = new List<string> { "Hello", "What can I do for you", "Oh hello", "What's up", "Hi" };

        internal static readonly Dictionary<string, Func<IDialogContext, string, Task<IEnumerable<Attachment>>>> _processCommands = new Dictionary<string, Func<IDialogContext, string, Task<IEnumerable<Attachment>>>>()
        {
            { _commandBaseHelp, ProcessCommands.BaseHelp },
            { _commandBaseNearby, ProcessCommands.BaseSearchNearby },
            { _commandBaseSignIn, ProcessCommands.BaseSignIn },
            { _commandBaseShareLocation, ProcessCommands.BaseShareLocation },
            { _commandBaseReservations, ProcessCommands.BaseReservation },

            { _commandVenueInfo, ProcessCommands.VenueInfo },
            { _commandVenueComments, ProcessCommands.VenueComments  },
            { _commandVenueReservations, ProcessCommands.VenueReservations },
            { _commandVenueResources, ProcessCommands.VenueResources },

            { _commandCommentsRead, ProcessCommands.CommentsRead },
            { _commandCommentsLike, ProcessCommands.CommentsLike },
            { _commandCommentsDislike, ProcessCommands.CommensDislike },
            { _commandCommentsAdd, ProcessCommands.CommensAdd },

            { _commandReservateAdd, ProcessCommands.ReservationAdd },
            { _commandReservateDelete, ProcessCommands.ReservationDelete }

            //{ _commandResourcesInfo, ProcessCommands.ResourceInfo },
        };

        internal static readonly List<string> _cardActionsYesNo = new List<string> { "yes", "no" };

        public static readonly JsonSerializerSettings _safeDeserializationSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            ContractResolver = new ReadOnlyJsonContractResolver(),
            Converters = new List<JsonConverter>
                    {
                        new Iso8601TimeSpanConverter()
                    }
        };

        internal static readonly MyAPI _api = new MyAPI(new Uri(_managerUri));

        public const string AuthTokenKey = "AuthToken";

        public const string facebookCallback =
#if DEBUG
            "https://193.198.16.210:44378/api/OAuthCallback";
#else
            "https://oqybot.azurewebsites.net/api/OAuthCallback";
#endif
    }
}