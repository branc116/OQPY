using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    public class WorkHours
    {
        public string Id { get; set; }

        public string VenueId { get; set; }
        public Venue Venue { get; set; }

        /// <summary>
        /// When the venue works.
        /// </summary>
        public List<WorkTime> WorkTimes { get; set; }

        public bool IsWorking { get; set; }
        public WorkTime Mondy => WorkTimes != null &&  WorkTimes.Count >= 1 ? WorkTimes[0] : null;
        public WorkTime Tuesday => WorkTimes != null && WorkTimes.Count >= 2 ? WorkTimes[1] : null;
        public WorkTime Wednesday => WorkTimes != null && WorkTimes.Count >= 3 ? WorkTimes[2] : null;
        public WorkTime Thursday => WorkTimes != null && WorkTimes.Count >= 4 ? WorkTimes[3] : null;
        public WorkTime Friday => WorkTimes != null && WorkTimes.Count >= 5 ? WorkTimes[4] : null;
        public WorkTime Saturday => WorkTimes != null && WorkTimes.Count >= 6 ? WorkTimes[5] : null;
        public WorkTime Sunday => WorkTimes != null && WorkTimes.Count >= 7 ? WorkTimes[6] : null;

        public Dictionary<DayOfWeek, WorkTime> WholeWeek => new Dictionary<DayOfWeek, WorkTime>()
        {
            {DayOfWeek.Monday, Mondy },
            {DayOfWeek.Tuesday, Tuesday },
            {DayOfWeek.Wednesday, Wednesday },
            {DayOfWeek.Thursday, Thursday },
            {DayOfWeek.Friday, Friday },
            {DayOfWeek.Saturday, Saturday },
            {DayOfWeek.Sunday, Sunday },
        };

        public WorkHours()
        {
        }

        public WorkHours(IEnumerable<WorkTime> workTimes, bool isWorking)
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkTimes = workTimes?.ToList() ?? null;
            this.IsWorking = isWorking;
        }

        public WorkHours(IEnumerable<WorkTime> workTimes, bool isWorking, Venue venue)
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkTimes = workTimes?.ToList() ?? null;
            this.IsWorking = isWorking;
            this.Venue = venue;
            this.VenueId = venue.Id;
        }

        public void FixLoops()
        {
            if (this.WorkTimes != null)
                foreach (var _ in WorkTimes)
                    _.WorkHours = this;
        }
        public void UnFixLoops()
        {
            if (this.WorkTimes != null)
                foreach (var _ in WorkTimes)
                    _.WorkHours = null;
        }
        public static IEnumerable<WorkHours> RandomWorkHours(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let workHours = new WorkHours(null, RandomBool())
                   let hours = workHours.WorkTimes = WorkTime.RandomWorkTime(7, workHours).ToList()
                   select workHours;
        }

    }
}