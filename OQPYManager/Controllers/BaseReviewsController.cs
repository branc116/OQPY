using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/BaseReviews")]
    public class BaseReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BaseReviews
        [HttpGet]
        public IEnumerable<BaseReview> GetReviews()
        {
            return _context.Reviews;
        }

        // GET: api/BaseReviews/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBaseReview([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseReview = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);

            if (baseReview == null)
            {
                return NotFound();
            }

            return Ok(baseReview);
        }

        // PUT: api/BaseReviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBaseReview([FromRoute] string id, [FromBody] BaseReview baseReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != baseReview.Id)
            {
                return BadRequest();
            }

            _context.Entry(baseReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaseReviewExists(id))
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

        // POST: api/BaseReviews
        [HttpPost]
        public async Task<IActionResult> PostBaseReview([FromBody] BaseReview baseReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reviews.Add(baseReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseReview", new { id = baseReview.Id }, baseReview);
        }

        // DELETE: api/BaseReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaseReview([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseReview = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            if (baseReview == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(baseReview);
            await _context.SaveChangesAsync();

            return Ok(baseReview);
        }

        private bool BaseReviewExists(string id) => _context.Reviews.Any(e => e.Id == id);

        [HttpGet]
        public async Task<IActionResult> GetBaseReviewForVenue([FromHeader] string venueId)
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

    }
}