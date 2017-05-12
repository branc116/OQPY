using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories.Interfaces
{
    interface IWorkHoursDbRepository : IBaseDbRepository<WorkHours>
    {
        /// <summary>
        /// Checks if a venue is working at current moment
        /// </summary>
        /// <param name="venue"></param>
        /// <returns>True if venue is working in open, false otherwise</returns>
        bool IsOpen(Venue venue);

        /// <summary>
        /// Change working status in case of some unpredictable events.
        /// E.g. death, holidays...
        /// </summary>
        void ChangeWorkingStatus();


    }
}
