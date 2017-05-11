using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class VenueTagDbRepository : BaseDbRepository<Tag>
    {
        public VenueTagDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.VenueTags;
        }

        public override async Task AddAsync(Tag tag, Venue venue)
        {
            var venueTag = new VenueTag(tag, venue);

            
        }

        public override Task<IQueryable<Tag>> Filter(Tag like)
        {
            throw new NotImplementedException();
        }
    }
}
