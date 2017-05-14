using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;
using System;
using System.Linq;
using System.Threading.Tasks;

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