using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// Reservation of an object.
    /// </summary>
    public class Reservation
    {
        public virtual string Id { get; set; }

        public virtual DateTime StartReservationTime { get; set; }

        public virtual DateTime EndReservationTime { get; set; }
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Could be just a name and/or surname
        /// </summary>
        public virtual string SecretCode { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return EndReservationTime - StartReservationTime;
            }
        }

        public Reservation()
        {
        }

        public Reservation(DateTime start, DateTime end)
        {
            Id = Guid.NewGuid().ToString();
            SecretCode = Guid.NewGuid().ToString();
            StartReservationTime = start;
            EndReservationTime = end;
        }

        public Reservation(DateTime start, DateTime end, Resource resource)
        {
            Id = Guid.NewGuid().ToString();
            SecretCode = Guid.NewGuid().ToString();
            StartReservationTime = start;
            EndReservationTime = end;
            this.Resource = resource;
        }

        public static IEnumerable<Reservation> RandomReservations(int n, Resource resource)
        {
            return from _ in new string(' ', n)
                   let start = DateTime.Now + RandomDays(5, 10)
                   let end = start + RandomHours(2, 5)
                   select new Reservation(start, end, resource);
        
        }
    }
}