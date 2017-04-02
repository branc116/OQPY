using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BaseWorkTime
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Diration => EndTime - StartTime;
        public BaseWorkHours WorkHours { get; set; }
        public BaseWorkTime()
        {

        }
        public BaseWorkTime(DateTime start, DateTime end)
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartTime = start;
            this.EndTime = end;
        }
        public BaseWorkTime(DateTime start, DateTime end, BaseWorkHours parent)
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartTime = start;
            this.EndTime = end;
            this.WorkHours = parent;
        }
        public static IEnumerable<BaseWorkTime> RandomWorkTime(int n, BaseWorkHours parent)
        {
            return from _ in new string(' ', n)
                   let startTime = new DateTime(RandomHours(4, 13).Ticks)
                   let endTime = new DateTime(RandomHours(16, 23).Ticks)
                   select new BaseWorkTime(startTime, endTime, parent);
        }
    }
}