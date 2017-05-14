using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Tags")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITagDbRepository _tagContext;

        public TagsController(ApplicationDbContext context,
                              ITagDbRepository tagContext)
        {
            _context = context;
            _tagContext = tagContext;
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
            if (!ModelState.IsValid)
            {
                base.BadRequest();
                return null;
            }

            var Tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);

            if (Tag == null)
                base.NotFound();
            else
                base.Ok();

            return Tag;
        }

        [HttpDelete]
        [Route("VenueTags")]
        public async Task<IActionResult> DeleteTagFromVenue([FromHeader] string venueId, [FromHeader] string tagValue)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("VenueTags")]
        public async Task<IActionResult> PostTagToVenue([FromHeader] string valueTag, [FromHeader] string venueId)
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
            var tag = new Tag(valueTag, venue);
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("VenueTags")]
        public async Task<IEnumerable<Tag>> GetTagsForVenue([FromHeader] string venueId)
        {
            if (!ModelState.IsValid)
            {
                base.BadRequest();
                return null;
            }
            var tag = _context.Venues.Include(i => i.Tags);
            var venue = await tag.FirstOrDefaultAsync((i) => i.Id == venueId);

            if (venue == null)
            {
                base.NotFound();
                return null;
            }

            var tags = from _ in venue.Tags
                       select _;

            if (tags == null)
                base.NotFound();
            else
                base.Ok();
            return tags;
        }
    }
}