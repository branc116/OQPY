using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYManager.Helper.Helper;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Reviews")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public IEnumerable<Review> GetReviews()
        {
            return _context.Reviews;
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);

            if (Review == null)
            {
                return NotFound();
            }

            return Ok(Review);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview([FromRoute] string id, [FromBody] Review Review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Review.Id)
            {
                return BadRequest();
            }

            _context.Entry(Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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
            if (!ModelState.IsValid)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            if (Review == null)
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
        public async Task<IActionResult> GetReviewFromVenue([FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviews = await _context.Venues
                .Where(i => i.Id == venueId)
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync();

            if (reviews == null)
            {
                return NotFound(venueId);
            }

            return Ok(reviews.Reviews);
        }

        [HttpPost]
        [Route("VenueReview")]
        public async Task<IActionResult> PostReviewToVenue([FromHeader] string comment, [FromBody] int rating, [FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Venue venue = await GetVenueAsync(_context, venueId);
            if (venue == null)
                return NotFound(venueId);
            var review = new Review(rating, comment, venue);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id });
        }

        [HttpDelete]
        [Route("VenueReview")]
        public async Task<IActionResult> DeleteReviewFromVenue([FromHeader] string reviewId, [FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == reviewId);
            if (Review == null)
                return NotFound(reviewId);

            var venue = await GetVenueAsync(_context, venueId, "Reviews");
            if (venue == null)
                return NotFound(venueId);

            int n = venue.Reviews.RemoveAll(i => i.Id == reviewId);
            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();

            return Ok(new { n = n, venueId = venueId, reviewId = reviewId });
        }
    }
}