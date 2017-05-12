using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// This class is used to describe a venue and enabling searching by specific filters.
    /// For example if a venue has pinball machine, the owner who creates venue will add tag "pinball".
    /// </summary>
    public class Tag: IComparer<Tag>
    {
        public virtual string Id { get; set; }

        public virtual string TagName { get; set; }

        /// <summary>
        /// This supports many-to-many relationship with venue
        /// </summary>
        public virtual List<VenueTag> VenueTags { get; set; }

        public Tag()
        {
        }

        public Tag(string tagName)
        {
            Id = Guid.NewGuid().ToString();
            this.TagName = tagName;
        }

        public Tag(string tagName, Venue venue)
        {
            Id = Guid.NewGuid().ToString();
            this.TagName = tagName;
            VenueTags = new List<VenueTag>();
            VenueTags.Add(new VenueTag(venue, this));
        }

        public int Compare(Tag x, Tag y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public static IEnumerable<Tag> RandomTags(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let tagName = RandomName()
                   let tag = new Tag(tagName)
                   let venuetag = tag.VenueTags = new List<VenueTag>() { new VenueTag(venue, tag) }
                   select tag;
        }

        internal void UnFixLoops()
        {
            VenueTags = null;
        }
    }
}