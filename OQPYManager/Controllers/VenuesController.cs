using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;

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

        public IEnumerable<Venue> GetAllVenues()
        {
            return _venuesDbRepository.GetAllVenues();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetVenue")]
        public IActionResult GetVenue(string id)
        {
            var item = _venuesDbRepository.FindVenue(id);
            if (item == null)
            {
                return NotFound();
            }
            
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult CreateVenue([FromBody] Venue venue)
        {
            if (venue == null)
            {
                return BadRequest();
            }

            _venuesDbRepository.AddVenue(venue);

            return CreatedAtRoute("GetVenue", new {id = venue.Id}, venue);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _venuesDbRepository.Remove(id);
        }
    }
}
