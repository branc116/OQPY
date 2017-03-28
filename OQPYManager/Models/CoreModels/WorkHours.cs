using System;
using System.Collections.Generic;

namespace OQPYManager.Models.CoreModels
{
    public class WorkHours
    {
        public string Id { get; set; }
        public Venue Venue { get; set; }
        /// <summary>
        /// When the venue works, must be nullable.
        /// </summary>
        public List<DateTime> WorkTimes { get; set; } = new List<DateTime>(7);
        public bool IsWorking { get; set; }
    }
}