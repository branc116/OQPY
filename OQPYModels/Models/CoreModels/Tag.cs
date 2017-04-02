using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BaseTag : IComparer<BaseTag>
    {
        public virtual string Id { get; set; }

        public virtual string TagName { get; set; }

        /// <summary>
        /// This supports many-to-many relationship with venue
        /// </summary>
        public virtual List<BaseVenueTag> VenueTags { get; set; }

        public BaseTag()
        {
        }

        public BaseTag(string tagName)
        {
            Id = Guid.NewGuid().ToString();
            this.TagName = tagName;
        }
        public BaseTag(string tagName, BaseVenue venue)
        {
            Id = Guid.NewGuid().ToString();
            this.TagName = tagName;
            VenueTags = new List<BaseVenueTag>() { new BaseVenueTag(this, venue) };
        }


        public int Compare(BaseTag x, BaseTag y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public static IEnumerable<BaseTag> RandomTags(int n, BaseVenue venue)
        {
            return from _ in new string(' ', n)
                   let tagName = RandomName()
                   let tag = new BaseTag(tagName)
                   let venuetag = tag.VenueTags = new List<BaseVenueTag>() { new BaseVenueTag(tag, venue) }
                   select tag;
                    
        }
    }
}