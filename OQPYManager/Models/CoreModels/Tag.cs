using System.Collections.Generic;

namespace OQPYManager.Models.CoreModels
{
    public class Tag
    {
        public string Id  { get; set; }

        public string TagName { get; set; }

        /// <summary>
        /// This supports many-to-many relationship with venue
        /// </summary>
        public List<VenueTag> VenueTags { get; set; }
    }

}