using System.Collections.Generic;

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

        public int Compare(BaseTag x, BaseTag y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}