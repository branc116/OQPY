using System;

namespace OQPYManager.Models.CoreModels
{
    public class WorkTime
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public WorkHours WorkHours { get; set; }
    }
}