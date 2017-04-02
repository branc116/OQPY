using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BaseReservation
    {
        public virtual string Id { get; set; }

        public virtual DateTime StartReservationTime { get; set; }

        public virtual DateTime EndReservationTime { get; set; }
        public virtual BaseResource Resource { get; set; }

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
        public BaseReservation()
        {

        }
        public BaseReservation(DateTime start, DateTime end)
        {
            Id = Guid.NewGuid().ToString();
            SecretCode = Guid.NewGuid().ToString();
            StartReservationTime = start;
            EndReservationTime = end;
        }
        public BaseReservation(DateTime start, DateTime end, BaseResource resource)
        {
            Id = Guid.NewGuid().ToString();
            SecretCode = Guid.NewGuid().ToString();
            StartReservationTime = start;
            EndReservationTime = end;
            this.Resource = resource;
        }
        public static IEnumerable<BaseReservation> RandomReservations(int n, BaseResource resource)
        {
            return from _ in new string(' ', n)
                   let start = DateTime.Now + RandomDays(5, 10)
                   let end = start + RandomHours(2, 5)
                   select new BaseReservation(start, end, resource);
        }
    }
}