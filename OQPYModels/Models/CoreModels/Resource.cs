using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    public class Resource
    {
        public virtual string Id { get; set; }

        /// <summary>
        /// Get one-to-many relationship with Venue
        /// </summary>
        public virtual Venue Venue { get; set; }

        /// <summary>
        /// Is it a billiards table, dining table, dartboard etc.
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// Owners name their stuff as they want
        /// </summary>
        public virtual string StuffName { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public Resource()
        {
        }

        public Resource(string suffName, string category)
        {
            this.StuffName = StuffName;
            this.Category = category;
            this.Id = Guid.NewGuid().ToString();
        }

        public Resource(string suffName, string category, Venue venue)
        {
            this.StuffName = StuffName;
            this.Category = category;
            this.Id = Guid.NewGuid().ToString();
            this.Venue = venue;
        }

        public static IEnumerable<Resource> RandomResources(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let rand = new Random()
                   let stuffName = RandomName()
                   let category = RandomName()
                   let resouce = new Resource(stuffName, category, venue)
                   let reserv = resouce.Reservations = Reservation.RandomReservations(rand.Next(5, 10), resouce).ToList()
                   select resouce;
        }
    }
}