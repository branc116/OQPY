using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    public class Resource: ICoreModel<Resource>
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

        /// <summary>
        /// maybe it'd be a good idea to have current state of the if it's iot enabled
        /// </summary>
        public bool OQPYed { get; set; }

        /// <summary>
        /// set this to true if the resource can change it's state by itself
        /// </summary>
        public bool IOTEnabled { get; set; }

        /// <summary>
        /// It'd be smart to have IOTEnabled resouce sends some sort of secret code. But for now it's null, not needed for now
        /// </summary>
        public string SecreteCode { get; set; }

        public List<Reservation> Reservations { get; set; }

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

        public string GetInfo()
        {
            var info = $"In: {Venue.Name}{Environment.NewLine}Taken: {OQPYed}{Environment.NewLine}Automated: {IOTEnabled}{Environment.NewLine}Number of reservations: {Reservations?.Count ?? 0}";
            return info;
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

        public bool Filter(Resource one, Resource two)
        {
            return one.Id == two.Id;
        }
    }
}