using System;
using System.Collections.Generic;
using System.Text;
using OQPYModels.Models.CoreModels;
using System.Linq;

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
        public static IEnumerable<BaseTag> IntersectionTags(this IEnumerable<BaseVenue> venues)
        {
            if (venues == null || !venues.Any())
                return null;
            IEnumerable<BaseTag> tags = venues.First().Tags;
            foreach (var _ in venues)
            {
                tags = tags.Intersect(_.Tags);
            }
            return tags;
        }

        public static string TagsToString(this IEnumerable<BaseTag> tags, Func<BaseTag, string> Stringify)
        {
            string retString = string.Empty;
            foreach (var _ in tags)
            {
                retString += Stringify(_);
            }
            return retString;
        }
    }
}
