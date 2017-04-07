using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Venues")]
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AllVenues
        [HttpGet]
        [Route("All")]
        public IEnumerable<Venue> GetAllVenues()
        {
            return from _ in _context.Venues
                   let OwnerName = _.Owner
                   let location = _.Location
                   select _;
        }

        //// PUT: api/Venues/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutVenue([FromRoute] string id, [FromBody] Venue venue)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != venue.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(venue).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!VenueExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // DELETE: api/Venues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venue = await _context.Venues.SingleOrDefaultAsync(m => m.Id == id);
            if (venue == null)
            {
                return NotFound();
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return Ok(venue);
        }

        private bool VenueExists(string id)
        {
            return _context.Venues.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("Multi")]
        public async Task<IActionResult> PostVenueMulti([FromBody] IEnumerable<string> names)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<Venue> venues;
            try
            {
                venues = from _ in names
                         let venue = Venue.CreateRandomVenues(1).First()
                         let name = venue.Name = _
                         select venue;
                _context.Venues.AddRange(venues);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            return CreatedAtAction("GetVenue", new { ids = names });
        }

        [HttpPost]
        [Route("Single")]
        public async Task<IActionResult> PostVenueSingle([FromBody] Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVenues", new { id = venue.Id }, venue);
        }

        [HttpGet]
        [Route("Multi")]
        public async Task<IActionResult> GetVenuesMulti([FromBody] IEnumerable<string> ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venues = from _ in _context.Venues
                         where ids.Any(i => i == _.Id)
                         select _;

            //var venue = from _ in ids
            //            join await _context.Venue.SingleOrDefaultAsync(m => m.Id == id) as

            if (venues == null)
            {
                return NotFound();
            }

            return Ok(venues);
        }

        [HttpGet]
        [Route("Single")]
        public async Task<IActionResult> GetVenueSingle([FromHeader] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venue = await _context.Venues.SingleOrDefaultAsync(m => m.Id == id);

            if (venue == null)
            {
                return NotFound();
            }

            return Ok(venue);
        }
    }
}