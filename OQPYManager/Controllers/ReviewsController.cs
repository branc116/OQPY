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
using static OQPYHelper.AuthHelper.Auth;
namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Reviews")]
    public class ReviewsController : Controller
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
        public IEnumerable<Review> GetReviews([FromHeader] string masterAdminKey)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                Ok();
                return _context.Reviews;
            }
            Unauthorized();
            return null;
        }

        // GET: api/Reviews/5
        [HttpGet]
        public async Task<Review> GetReview([FromHeader] string reviewId)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
                return null;
            }

            var review = await _reviewDbRepository.FindAsync(reviewId);

            if (review == null)
                NotFound(reviewId);
            else
                Ok();
            return review;
        }
        [HttpGet]
        [Route("VenueReview")]
        public async Task<IEnumerable<Review>> GetReviewFromVenue([FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
                return null;
            }

            var reviews = await _context.Venues
                .Where(i => i.Id == venueId)
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync();

            if (reviews == null)
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
        public async Task<IActionResult> PostReviewToVenue([FromHeader] string comment, [FromHeader] string venueId,
            [FromBody] int rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var venue = await _context.Venues
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync(i => i.Id == venueId);
            if (venue == null)
                return NotFound(venueId);
            var review = new Review(rating, comment, venue);

            venue.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("VenueReview")]
        public async Task<IActionResult> DeleteReviewFromVenue([FromHeader] string reviewId, [FromHeader] string masterAdminKey)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == reviewId);
                if (Review == null)
                    return NotFound(reviewId);
                _context.Reviews.Remove(Review);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Unauthorized();
        }

        /// <summary>
        /// One can like or dislike a review, true = like, false = dislike
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("like")]
        public async Task<IActionResult> LikeReview([FromHeader]string reviewId, [FromHeader]string like)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return NotFound();
            review.Helpfulness += like == "0" ? -1 : 1;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
 