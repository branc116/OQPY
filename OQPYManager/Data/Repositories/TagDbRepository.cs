using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class TagDbRepository : BaseDbRepository<Tag>, ITagDbRepository
    {
        private readonly VenuesDbRepository _venuesDbRepository;

        public TagDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Tags;
            _venuesDbRepository = new VenuesDbRepository(context);
        }



        public async Task AddAsync(Tag tag, string venueId)
        {
            var venue = await _venuesDbRepository.FindAsync(venueId);

            var venueTag = new VenueTag(venue, tag);

            venue.VenueTags.Add(venueTag);
            tag.VenueTags.Add(venueTag);
            _defaultDbSet.Add(tag);
            await _venuesDbRepository.UpdateAsync(venue);
        }

        public override Task<IQueryable<Tag>> Filter(Tag like)
        {
            throw new NotImplementedException();
        }
    }
}
