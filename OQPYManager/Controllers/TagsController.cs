using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Tags")]
    public class TagsController: Controller
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        [Route("All")]
        public IEnumerable<Tag> GetTags()
        {
            return _context.Tags;
        }

        // GET: api/Tags/5
        [HttpGet]
        public async Task<Tag> GetTag([FromHeader] string id)
        {
            if ( !ModelState.IsValid )
            {
                base.BadRequest();
                return null;
            }

            var Tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);

            if ( Tag == null )
                base.NotFound();
            else
                base.Ok();

            return Tag;
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag([FromRoute] string id, [FromBody] Tag Tag)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            if ( id != Tag.Id )
            {
                return BadRequest();
            }

            _context.Entry(Tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch ( DbUpdateConcurrencyException )
            {
                if ( !TagExists(id) )
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

        // POST: api/Tags
        [HttpPost]
        public async Task<IActionResult> PostTag([FromBody] Tag Tag)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            _context.Tags.Add(Tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTag", new { id = Tag.Id }, Tag);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] string id)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            var Tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);
            if ( Tag == null )
            {
                return NotFound();
            }

            _context.Tags.Remove(Tag);
            await _context.SaveChangesAsync();

            return Ok(Tag);
        }

        private bool TagExists(string id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }

        [HttpDelete]
        [Route("VenueTags")]
        public async Task<IActionResult> DeleteTagFromVenue([FromHeader] string venueId, [FromHeader] string tagValue)
        {
            if ( !ModelState.IsValid )
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
            if ( venue == null )
            {
                return Ok();
            }
            int n = venue.Tags.RemoveAll((i) => i.TagName == tagValue);
            await _context.SaveChangesAsync();

            return Ok(n);
        }

        [HttpPost]
        [Route("VenueTags")]
        public async Task<IActionResult> PostTagToVenue([FromHeader] string valueTag, [FromHeader] string venueId)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            var venue = await _context.Venues.Where(i => i.Id == venueId).Include(i => i.Tags).FirstOrDefaultAsync();
            if ( venue == null )
            {
                return BadRequest(new { VenueId = venueId });
            }
            var tag = new Tag(valueTag, venue);
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("VenueTags")]
        public async Task<IEnumerable<Tag>> GetTagsForVenue([FromHeader] string venueId)
        {
            if ( !ModelState.IsValid )
            {
                base.BadRequest();
                return null;
            }
            var tag = _context.Venues.Include(i => i.Tags);
            var venue = await tag.FirstOrDefaultAsync((i) => i.Id == venueId);

            if ( venue == null )
            {
                base.NotFound();
                return null;
            }

            var tags = from _ in venue.Tags
                       select _;

            if ( tags == null )
                base.NotFound();
            else
                base.Ok();
            return tags;
        }
    }
}