using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class WorkHoursDbRepository : BaseDbRepository<WorkHours>, IWorkHoursDbRepository
    {
        private readonly VenuesDbRepository _venuesDbRepository;

        public WorkHoursDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.VenueWorkHours;
            _venuesDbRepository = new VenuesDbRepository(context);
        }

        public override Task<IQueryable<WorkHours>> Filter(WorkHours like)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsOpenAsync(string venueId)
        {
            var now = DateTime.Now;
            var venue = await _venuesDbRepository.FindAsync(venueId);

            if (venue.WorkHours.IsWorking == false)
            {
                return false;
            }

            var workTime = venue.WorkHours.WholeWeek.FirstOrDefault(ww => ww.Key.Equals(now.DayOfWeek)).Value;

            return IsTimeInBetweenWorkingTime(now, workTime);

        }

        public async Task ChangeWorkingStatusAsync(string venueId)
        {
            var venue = await _venuesDbRepository.FindAsync(venueId);
            venue.WorkHours.IsWorking = !venue.WorkHours.IsWorking;
            await _venuesDbRepository.UpdateAsync(venue);
        }

        private bool IsTimeInBetweenWorkingTime(DateTime time, WorkTime worktime)
        {
            //I've done this to avoid any influence of calendar date into calculation
            return time.Hour.CompareTo(worktime.StartTime.Hour) < 0
                   && time.Minute.CompareTo(worktime.StartTime.Minute) < 0
                   && time.Hour.CompareTo(worktime.EndTime.Hour) > 0
                   && time.Minute.CompareTo(worktime.EndTime.Minute) > 0;
        }


        
    }
}
