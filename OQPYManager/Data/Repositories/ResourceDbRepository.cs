using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYManager.Helper;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class ResourceDbRepository : BaseDbRepository<Resource>, IResourceDbRepository
    {
        private const string TAG = "ResourceDb";

        public ResourceDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Resources;
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
                .FirstOrDefaultAsync(v => v.Id.Equals(venueId));
            if (venue != null) return venue.Resources;
            Log.BasicLog(TAG, $"Get all venues id not found = {venueId}", SeverityLevel.Error);
            throw new KeyNotFoundException(venueId);
        }

        public override Task<IQueryable<Resource>> Filter(Resource like)
        {
            throw new NotImplementedException();
        }

    }
}