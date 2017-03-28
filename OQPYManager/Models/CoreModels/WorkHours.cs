using System;
using System.Collections.Generic;

namespace OQPYManager.Models.CoreModels
{
    public class WorkHours
    {
        public string Id { get; set; }

        public string VenueId { get; set; }
        public Venue Venue { get; set; }

        /// <summary>
        /// When the venue works.
        /// </summary>
        public List<WorkTime> WorkTimes { get; set; }
        public bool IsWorking { get; set; }
    }
}