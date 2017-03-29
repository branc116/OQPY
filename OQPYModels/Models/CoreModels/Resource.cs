using System.Collections.Generic;

namespace OQPYModels.Models.CoreModels
{
    public class BaseResource
    {
        public virtual string Id { get; set; }

        /// <summary>
        /// Get one-to-many relationship with Venue
        /// </summary>
        public virtual BaseVenue Venue { get; set; }

        /// <summary>
        /// Is it a billiards table, dining table, dartboard etc.
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// Owners name their stuff as they want
        /// </summary>
        public virtual string StuffName { get; set; }

        public virtual List<BaseReservation> Reservations { get; set; }
    }
}