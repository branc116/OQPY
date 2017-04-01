using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BaseWorkHours
    {
        public string Id { get; set; }

        public string VenueId { get; set; }
        public BaseVenue Venue { get; set; }

        /// <summary>
        /// When the venue works.
        /// </summary>
        public List<BaseWorkTime> WorkTimes { get; set; }

        public bool IsWorking { get; set; }
        public BaseWorkTime Mondy => WorkTimes?[0] ?? null;
        public BaseWorkTime Tuesday => WorkTimes?[1] ?? null;
        public BaseWorkTime Wednesday => WorkTimes?[2] ?? null;
        public BaseWorkTime Thursday => WorkTimes?[3] ?? null;
        public BaseWorkTime Friday => WorkTimes?[4] ?? null;
        public BaseWorkTime Saturday => WorkTimes?[5] ?? null;
        public BaseWorkTime Sunday => WorkTimes?[6] ?? null;
        public BaseWorkHours(IEnumerable<BaseWorkTime> workTimes, bool isWorking)
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkTimes = workTimes?.ToList() ?? null;
            this.IsWorking = isWorking;
        }
        public BaseWorkHours(IEnumerable<BaseWorkTime> workTimes, bool isWorking, BaseVenue venue)
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkTimes = workTimes?.ToList() ?? null;
            this.IsWorking = isWorking;
            this.Venue = venue;
            this.VenueId = venue.Id;
        }
        public static IEnumerable<BaseWorkHours> RandomWorkHours(int n, BaseVenue venue)
        {
            return from _ in new string(' ', n)
                   let workHours = new BaseWorkHours(null, RandomBool())
                   let hours = workHours.WorkTimes = BaseWorkTime.RandomWorkTime(7, workHours).ToList()
                   select workHours;
        }
    }
}
