using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
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

        public BaseResource(string suffName, string category)
        {
            this.StuffName = StuffName;
            this.Category = category;
            this.Id = Guid.NewGuid().ToString();
        }
        public BaseResource(string suffName, string category, BaseVenue venue)
        {
            this.StuffName = StuffName;
            this.Category = category;
            this.Id = Guid.NewGuid().ToString();
            this.Venue = venue;
        }
        public static IEnumerable<BaseResource> RandomResources(int n, BaseVenue venue)
        {
            return from _ in new string(' ', n)
                   let rand = new Random()
                   let stuffName = RandomName()
                   let category = RandomName()
                   let resouce = new BaseResource(stuffName, category, venue)
                   let reserv = resouce.Reservations = BaseReservation.RandomReservations(rand.Next(5, 10), resouce).ToList()
                   select resouce;
        }
    }
}