using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYManager.Helper;

namespace OQPYManager.Data.Repositories
{
    using Base;
    using OQPYModels.Models.CoreModels;

    public class ResourceDbRepository : BaseDbRepository<Resource>, IResourceDbRepository
    {
        private const string TAG = "ResourceDb";

        public ResourceDbRepository(ApplicationDbContext context) : base(context)
        {
        }

       
        public async Task ChangeState(string id, bool newState, string secretCode)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                Log.BasicLog(TAG, $"Change state id not found = {id}", SeverityLevel.Error);
                throw new KeyNotFoundException("id");
            }
            resource.OQPYed = newState;
            _context.Update(resource);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Resource>> GetAllInVenue(string venueId)
        {
            var venue = await _context.Venues
                .Include(i => i.Resources)
                .FirstOrDefaultAsync();
            if (venue == null)
            {
                Log.BasicLog(TAG, $"Get all venues id not found = {venueId}", SeverityLevel.Error);
                throw new KeyNotFoundException(venueId);
            }
            return venue.Resources;
        }

        public override Task<IQueryable<Resource>> Filter(Resource like)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Resource> GetAll(DbSet<Resource> dbSet = null)
        {
            return base.GetAll(_context.Resources);
        }

        public override IEnumerable<Resource> GetAll(DbSet<Resource> dbSet = null, params string[] includedParams)
        {
            return base.GetAll(_context.Resources, includedParams);
        }

        public override IEnumerable<Resource> GetAll(string includedParams, DbSet<Resource> dbSet = null)
        {
            return base.GetAll(includedParams, _context.Resources);
        }

        public override IEnumerable<Resource> Get(DbSet<Resource> dbSet = null, params Func<Resource, bool>[] filters)
        {
            return base.Get(_context.Resources, filters);
        }

        public override IEnumerable<Resource> Get(string includedParams, DbSet<Resource> dbSet = null, params Func<Resource, bool>[] filters)
        {
            return base.Get(includedParams, _context.Resources, filters);
        }
    }
}
