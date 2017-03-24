using System.Collections.Generic;

namespace OQPYManager.Models.CoreModels
{
    public class Resource
    {
        public string Id { get; set; }

        /// <summary>
        /// Get one-to-many relationship with Venue
        /// </summary>
        public Venue Venue { get; set; }

        /// <summary>
        /// Is it a billiards table, dining table, dartboard etc.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Owners name their stuff as they want
        /// </summary>
        public string StuffName { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}