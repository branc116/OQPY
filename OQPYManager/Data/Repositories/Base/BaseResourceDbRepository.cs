using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using static OQPYManager.Helper.Log;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseResourceDbRepository: BaseDbRepository<Resource>, IResourceDbRepository
    {
        private const string TAG = "BaseResource";
        public BaseResourceDbRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task ChangeState(string id, bool newState, string secretCode)
        {
            var resource = await _context.Resources.FindAsync(id);
            if ( resource == null )
            {
                BasicLog(TAG, $"Change state id not found = {id}", SeverityLevel.Error);
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
            if ( venue == null )
            {
                BasicLog(TAG, $"Get all venues id not found = {venueId}", SeverityLevel.Error);
                throw new KeyNotFoundException(venueId);
            }
            return venue.Resources;
        }
    }
}
