using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class ReviewDbRepository : BaseDbRepository<Review>, IReviewDbRepository
    {
        private readonly VenuesDbRepository _venuesDbRepository;
        public ReviewDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Reviews;
            _venuesDbRepository = new VenuesDbRepository(context);
        }


        public override Task<IQueryable<Review>> Filter(Review like)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Review>> GetAllReviews(string venueId)
        {
            var venue = await _venuesDbRepository.FindAsync(venueId);

            return venue.Reviews;
        }

        public async Task RateReview(string reviewId, string like)
        {
            var review = await _context.Reviews
                       .FirstOrDefaultAsync(r => r.Id.Equals(reviewId));

            review.Helpfulness += like.Equals("0") ? -1 : 1;
            _context.SaveChanges();

        }

        public async Task AddNewReview(string venueId, Review review)
        {
            var venue = await _venuesDbRepository.FindAsync(venueId);
            venue.Reviews.Add(review);
            await _venuesDbRepository.UpdateAsync(venue);
        }
    }
}
