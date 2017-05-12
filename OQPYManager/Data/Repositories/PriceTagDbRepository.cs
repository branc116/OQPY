using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class PriceTagDbRepository : BaseDbRepository<PriceTag>, IPriceTagDbRepository
    {
        private VenuesDbRepository _venuesDbRepository;
        public PriceTagDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.PriceTags;
            _venuesDbRepository = new VenuesDbRepository(context);
        }

        public override Task<IQueryable<PriceTag>> Filter(PriceTag like)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PriceTag>> GetPriceTagsForVenue(string venueId)
        {
            var venue = await _venuesDbRepository.FindAsync(venueId);

            return venue.PriceTags;
        }
    }
}
