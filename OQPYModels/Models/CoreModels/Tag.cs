using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
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

        

        public int Compare(Tag x, Tag y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public static IEnumerable<Tag> RandomTags(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let tagName = RandomName()
                   let tag = new Tag(tagName)
                   let venuetag = tag.VenueTags = new List<VenueTag>() { new VenueTag(TODO, TODO, venue) }
                   select tag;
        }

        internal void UnFixLoops()
        {
            VenueTags = null;
        }
    }
}