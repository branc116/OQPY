using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// This class describes more in detail about working time of a venue in one day.
    /// </summary>
    public class WorkTime
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public WorkHours WorkHours { get; set; }

        public WorkTime()
        {
        }

        public WorkTime(DateTime start, DateTime end)
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartTime = start;
            this.EndTime = end;
        }

        public WorkTime(DateTime start, DateTime end, WorkHours parent)
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartTime = start;
            this.EndTime = end;
            this.WorkHours = parent;
        }

        public static IEnumerable<WorkTime> RandomWorkTime(int n, WorkHours parent)
        {
            return from _ in new string(' ', n)
                   let startTime = new DateTime(RandomHours(4, 13).Ticks)
                   let endTime = new DateTime(RandomHours(16, 23).Ticks)
                   select new WorkTime(startTime, endTime, parent);
        }
    }
}