using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories;
using OQPYManager.Data.Repositories.Interfaces;
using static OQPYManager.Helper.Helper;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Reviews")]
    public class ReviewsController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReviewDbRepository _reviewDbRepository;

        public ReviewsController(ApplicationDbContext context, IReviewDbRepository reviewDbRepository)
        {
            _context = context;
            _reviewDbRepository = reviewDbRepository;
        }

        // GET: api/Reviews
        [HttpGet]
        [Route("All")]
        public IEnumerable<Review> GetReviews()
        {
            return _context.Reviews;
        }

        // GET: api/Reviews/5
        [HttpGet]
        public async Task<Review> GetReview([FromHeader] string reviewId)
        {
            if ( !ModelState.IsValid )
            {
                BadRequest(ModelState);
                return null;
            }

            var review = await _reviewDbRepository.FindAsync(reviewId);

            if ( review == null )
                NotFound(reviewId);
            else
                Ok();
            return review;
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview([FromRoute] string id, [FromBody] Review Review)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            if ( id != Review.Id )
            {
                return BadRequest();
            }

            _context.Entry(Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch ( DbUpdateConcurrencyException )
            {
                if ( !ReviewExists(id) )
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<IActionResult> PostReview([FromBody] Review Review)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = Review.Id }, Review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] string id)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            if ( Review == null )
            {
                return NotFound();
            }

            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();

            return Ok(Review);
        }

        private bool ReviewExists(string id) => _context.Reviews.Any(e => e.Id == id);

        [HttpGet]
        [Route("VenueReview")]
        public async Task<IEnumerable<Review>> GetReviewFromVenue([FromHeader] string venueId)
        {
            if ( !ModelState.IsValid )
            {
                BadRequest(ModelState);
                return null;
            }

            var reviews = await _context.Venues
                .Where(i => i.Id == venueId)
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync();

            if ( reviews == null )
            {
                NotFound(venueId);
                return null;
            }
            else
                Ok();
            return reviews.UnFixLoops().Reviews;
        }

        [HttpPost]
        [Route("VenueReview")]
        public async Task<IActionResult> PostReviewToVenue([FromHeader] string comment, [FromHeader] string venueId, [FromBody] int rating)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            var venue = await _context.Venues
                .FirstOrDefaultAsync(i => i.Id == venueId);
            if ( venue == null )
                return NotFound(venueId);
            var review = new Review(rating, comment, venue);
            await _context.AddAsync(review);
            //await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("VenueReview")]
        public async Task<IActionResult> DeleteReviewFromVenue([FromHeader] string reviewId, [FromHeader] string venueId)
        {
            if ( !ModelState.IsValid )
                return BadRequest(ModelState);

            var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == reviewId);
            if ( Review == null )
                return NotFound(reviewId);

            var venue = await GetVenueAsync(_context, venueId, "Reviews");
            if ( venue == null )
                return NotFound(venueId);

            int n = venue.Reviews.RemoveAll(i => i.Id == reviewId);
            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();

            return Ok(new { n = n, venueId = venueId, reviewId = reviewId });
        }

        /// <summary>
        /// One can like or dislike a review, true = like, false = dislike
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("like")]
        public async Task<IActionResult> LikeReview([FromHeader]string reviewId,[FromBody]string like)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if ( review == null )
                return NotFound();
            review.Helpfulness += like == "0" ? -1 : 1;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}