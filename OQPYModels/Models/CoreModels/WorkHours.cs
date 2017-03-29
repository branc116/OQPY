using System.Collections.Generic;

namespace OQPYModels.Models.CoreModels
{
    public class BaseWorkHours
    {

        public string Id { get; set; }

        public string VenueId { get; set; }
        public BaseVenue Venue { get; set; }

        /// <summary>
        /// When the venue works.
        /// </summary>
        public List<BaseWorkTime> WorkTimes { get; set; }
        public bool IsWorking { get; set; }
    }
}