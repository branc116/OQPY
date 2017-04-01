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
    [Route("api/BaseTags")]
    public class BaseTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BaseTags
        [HttpGet]
        public IEnumerable<BaseTag> GetTags()
        {
            return _context.Tags;
        }

        // GET: api/BaseTags/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBaseTag([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseTag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);

            if (baseTag == null)
            {
                return NotFound();
            }

            return Ok(baseTag);
        }

        [HttpGet]
        [Route("VenueTags")]
        public async Task<IEnumerable<BaseTag>> GetBaseTagsForVenue([FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var tag = _context.Venues.Include(i => i.Tags);
            var venue = await tag.FirstOrDefaultAsync((i) => i.Id == venueId);
            
            
            if (venue == null)
            {
                return null;
            }

            var tags = from _ in venue.Tags
                       select _;
            

            if (tags == null)
            {
                return null;
            }

            return tags;
        }

        // PUT: api/BaseTags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBaseTag([FromRoute] string id, [FromBody] BaseTag baseTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != baseTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(baseTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaseTagExists(id))
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

        // POST: api/BaseTags
        [HttpPost]
        public async Task<IActionResult> PostBaseTag([FromBody] BaseTag baseTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tags.Add(baseTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseTag", new { id = baseTag.Id }, baseTag);
        }
        [HttpPost]
        [Route("VenueTags")]
        public async Task<IActionResult> PostBaseTagToVenue([FromHeader] string valueTag, [FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var venue = await _context.Venues.Where(i => i.Id == venueId).Include(i => i.Tags).FirstOrDefaultAsync();
            if (venue == null)
            {
                return BadRequest(new { VenueId = venueId });
            }
            var tag = new BaseTag(valueTag, venue);
            //venue.Tags.Add(tag);
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseTag", new { id = tag.Id }, tag);
        }

        // DELETE: api/BaseTags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaseTag([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseTag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);
            if (baseTag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(baseTag);
            await _context.SaveChangesAsync();

            return Ok(baseTag);
        }
        [HttpDelete]
        [Route("VenueTags")]
        public async Task<IActionResult> DeleteBaseTagFromVenue([FromHeader] string venueId, [FromHeader] string tagValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var venueTags = _context.Tags.Include(i => i.VenueTags).Where(i => i.TagName == tagValue);
            //foreach(var vt in venueTags)
            //{
            //    vt.VenueTags.RemoveAll(i => i.VenueId == venueId);
            //}
            var venue = await _context.Venues
                .Where(i => i.Id == venueId)
                .Include(i => i.Tags)
                .FirstOrDefaultAsync();
            if (venue == null)
            {
                return Ok();
            }
            int n = venue.Tags.RemoveAll((i) => i.TagName == tagValue);
            await _context.SaveChangesAsync();

            return Ok(n);
        }

        private bool BaseTagExists(string id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}