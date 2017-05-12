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

        public bool IsOpen(string venueId)
        {
            var venue = _venuesDbRepository.FindAsync(venueId);
            DateTime.Now.DayOfWeek.;
        }

        public void ChangeWorkingStatus()
        {
            throw new NotImplementedException();
        }
    }
}
