using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data.Interface;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OQPYManager.Controllers
{
    [Route("api/venues")]
    public class VenuesController : Controller
    {
        private readonly IVenuesDbRepository _venuesDbRepository;

        public VenuesController(IVenuesDbRepository venuesDbRepository)
        {
            _venuesDbRepository = venuesDbRepository;
        }

        [HttpGet]
        public IEnumerable<Venue> GetAllVenues()
        {
            return _venuesDbRepository.GetAllVenues();
        }

        [HttpPost]
        public IActionResult CreateVenue([FromBody] Venue venue)
        {
            if (venue == null)
            {
                return BadRequest();
            }

            _venuesDbRepository.AddVenueAsync(venue);

            return CreatedAtRoute("GetVenue", new { id = venue.Id }, venue);
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete([FromHeader] string id)
        {
            _venuesDbRepository.RemoveAsync(id);
        }

        [HttpPost]
        [Route("Multi")]
        public async Task<IActionResult> PostVenueMulti([FromBody] IEnumerable<string> names)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var venues = from _ in names
                         let venue = Venue.CreateRandomVenues(1).First()
                         let name = venue.Name = _
                         select venue;
            await _venuesDbRepository.AddVenuesAsync(venues);
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

            await _venuesDbRepository.AddVenueAsync(venue);
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

            var venues = _venuesDbRepository.GetVenues(i => ids.Contains(i.Id));

            if (venues == null)
                return NotFound();

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

            var venue = await _venuesDbRepository.FindVenueAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            return Ok(venue);
        }
    }
}