using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class LocationDbRepository : BaseDbRepository<Location>
    {
        public LocationDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Locations;
        }

        public override Task<IQueryable<Location>> Filter(Location like)
        {
            throw new NotImplementedException();
        }
    }
}
