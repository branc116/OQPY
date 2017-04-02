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
        public WorkTime Mondy => WorkTimes?[0] ?? null;
        public WorkTime Tuesday => WorkTimes?[1] ?? null;
        public WorkTime Wednesday => WorkTimes?[2] ?? null;
        public WorkTime Thursday => WorkTimes?[3] ?? null;
        public WorkTime Friday => WorkTimes?[4] ?? null;
        public WorkTime Saturday => WorkTimes?[5] ?? null;
        public WorkTime Sunday => WorkTimes?[6] ?? null;

        public Dictionary<DayOfWeek, WorkTime> WholeWeek => new Dictionary<DayOfWeek, WorkTime>()
        {
            {DayOfWeek.Monday, WorkTimes?[0] ?? null },
            {DayOfWeek.Tuesday, WorkTimes?[1] ?? null },
            {DayOfWeek.Wednesday, WorkTimes?[2] ?? null },
            {DayOfWeek.Thursday, WorkTimes?[3] ?? null },
            {DayOfWeek.Friday, WorkTimes?[4] ?? null },
            {DayOfWeek.Saturday, WorkTimes?[5] ?? null },
            {DayOfWeek.Sunday, WorkTimes?[6] ?? null },
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

        public static IEnumerable<WorkHours> RandomWorkHours(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let workHours = new WorkHours(null, RandomBool())
                   let hours = workHours.WorkTimes = WorkTime.RandomWorkTime(7, workHours).ToList()
                   select workHours;
        }
    }
}