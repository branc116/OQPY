using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OQPYBot.Controllers.Helper
{
    public static class Constants
    {
        public const string _propNames = "propertyNames";
        public const string _name = "name";
        public const string _email = "email";
        public const string _location = "location";
        public const string _facebooklocation = "Facebook Location";
        public const string _channelId = "chId";
        public const string _subsublocation = "SubSubLocation";
        public const string _currentActiveVenues = "currentActiveVenues";
        public const string _insideDialogKey = "insideProperty";
        public static readonly List<string> _propertyKeys = new List<string> { _name, _email, _location, _facebooklocation, _currentActiveVenues };
        public static readonly List<string> _cardActions = new List<string> { "Info", "Comments", "Reservations"};
        public static readonly List<string> _cardActionsYesNo = new List<string> { "yes", "no" };
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
    }
}