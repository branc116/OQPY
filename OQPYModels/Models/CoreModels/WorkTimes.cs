using System;

namespace OQPYModels.Models.CoreModels
{
    public class BaseWorkTime
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BaseWorkHours WorkHours { get; set; }
    }
}