using OQPYModels.Helper;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OQPYModels.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Get tags that are contained in all of the Venues.
        /// E.g. Venue1.tags = ["tag1", "tag2"], Venue2.tags = ["tag2","tag3"], return set will be ["tag2"]
        /// </summary>
        /// <param name="venues">Set of venues</param>
        /// <returns>Set of common tags</returns>
        public static IEnumerable<Tag> IntersectionTags(this IEnumerable<Venue> venues)
        {
            if ( venues == null || !venues.Any() )
                return null;
            IEnumerable<Tag> tags = venues.First().Tags;
            foreach ( var _ in venues )
            {
                tags = tags.Intersect(_.Tags);
            }
            return tags;
        }

        public static string TagsToString(this IEnumerable<Tag> tags, Func<Tag, string> Stringify)
        {
            string retString = string.Empty;
            if ( tags != null )
                foreach ( var _ in tags )
                {
                    retString += Stringify(_);
                }
            return retString;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> _, T Like) where T : ICoreModel<T>
        {
            //T.Filter
            //typeof(T).GetProperties().Where(i => i?.GetCustomAttribute<FilterableAttribute>()?.Filter ?? false).Select((i) => i.GetValue(;
            //from __ in typeof(T).GetProperties()
            //where __?.GetCustomAttribute<FilterableAttribute>()?.Filter ?? false
            //where __?.GetValue
            return (from __ in _
                    where __.Filter(__, Like)
                    select __).AsQueryable();
        }

        public static bool Working(this WorkHours obj, DateTime from, DateTime to)
        {
            var working = true;
            if ( obj != null )
            {
                var today = obj?.WholeWeek?[from.DayOfWeek];
                if ( today != null )
                {
                    working = today.StartTime.Hour < from.Hour && today.EndTime.Hour > from.Hour;
                }
            }
            return obj.IsWorking && working;
        }

        public static bool Working(this WorkHours obj, DateTime dateTime)
        {
            return obj.Working(dateTime, dateTime);
        }

        public static bool Reservable(this Resource obj, DateTime from, DateTime to)
        {
            return obj.Reservations
                .Any(i => i.StartReservationTime < from && i.EndReservationTime > from ||
                          i.StartReservationTime < to && i.EndReservationTime > to);
        }
    }
}