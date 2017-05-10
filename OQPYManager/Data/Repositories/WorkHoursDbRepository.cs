using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class WorkHoursDbRepository : BaseDbRepository<WorkHours>
    {
        public WorkHoursDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.VenueWorkHours;
        }

        public override Task<IQueryable<WorkHours>> Filter(WorkHours like)
        {
            throw new NotImplementedException();
        }
    }
}
