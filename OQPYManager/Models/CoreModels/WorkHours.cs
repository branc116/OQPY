
using System;
using System.Collections.Generic;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Models.CoreModels
{
    public class WorkHours : BaseWorkHours
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