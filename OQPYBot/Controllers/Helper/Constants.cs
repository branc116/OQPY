using System.Collections.Generic;

namespace OQPYBot.Controllers.Helper
{
    public static class Constants
    {
        public const string _propNames = "propertyNames";
        public const string _name = "name";
        public const string _email = "email";
        public const string _location = "location";
        public const string _sublocation = "SubLocation";
        public const string _subsublocation = "SubSubLocation";
        public static readonly List<string> _propertyKeys = new List<string> { "name", "email", "location" };
        public static readonly List<string> _cardActions = new List<string> { "Info", "Comments", "Reservate", "See list of reservations" };
        public static readonly List<string> _cardActionsYesNo = new List<string> { "yes", "no" };
    }
}