﻿using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYHelper.AuthHelper.Auth;

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
        [Route("All")]
        public IEnumerable<Venue> GetAllVenues([FromHeader] string masterAdminKey)
        {
            if (!ValidateMasterAdminKey(masterAdminKey))
            {
                Unauthorized();
                return null;
            }

            return _venuesDbRepository.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenue([FromHeader] string masterAdminKey, [FromBody] Venue venue)
        {
            if (!ValidateMasterAdminKey(masterAdminKey))
                return Unauthorized();

            if (venue == null)
            {
                return BadRequest();
            }

            await _venuesDbRepository.AddAsync(venue);

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task Delete([FromHeader] string id, [FromHeader] string masterAdminKey)
        {
            if (!ValidateMasterAdminKey(masterAdminKey))
                Unauthorized();

            try
            {
                await _venuesDbRepository.RemoveAsync(id);
                Ok("Deleted");
            }
            catch (Exception ex)
            {
                BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("Multi")]
        public async Task<IActionResult> PostVenueMulti([FromHeader] string masterAdminKey, [FromBody] IEnumerable<string> names)
        {
            if (!ValidateMasterAdminKey(masterAdminKey))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var venues = from _ in names
                         let venue = Venue.CreateRandomVenues(1).First()
                         let name = venue.Name = _
                         select venue;
            await _venuesDbRepository.AddAsync(venues);
            return Ok();
        }

        [HttpPost]
        [Route("MultiFull")]
        public async Task<IActionResult> PostVenueMulti([FromHeader] string masterAdminKey, [FromBody] IEnumerable<Venue> venues)
        {
            if (!ValidateMasterAdminKey(masterAdminKey))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _venuesDbRepository.AddAsync(from _ in venues select _.FixLoops());
            return Ok();
        }

        [HttpGet]
        [Route("Multi")]
        public async Task<IEnumerable<Venue>> GetVenuesMulti([FromBody] IEnumerable<string> ids)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
                return null;
            }
            if (ids == null)
            {
                BadRequest();
                return null;
            }

            var venues = _venuesDbRepository.Get(i => ids.Contains(i.Id));

            if (venues == null)
            {
                base.NotFound();
                return null;
            }
            else
                base.Ok();
            foreach (var _ in venues)
                _.UnFixLoops();
            return venues;
        }

        [HttpGet]
        [Route("Single")]
        public async Task<Venue> GetVenueSingle([FromHeader] string id)
        {
            if (!ModelState.IsValid)
            {
                base.BadRequest();
                return null;
            }

            var venue = await _venuesDbRepository.FindAsync(id);

            if (venue == null)
            {
                base.NotFound();
                return null;
            }
            else
                base.Ok();
            return venue.UnFixLoops();
        }

        [HttpPost]
        [Route("Filter")]
        public async Task<IEnumerable<Venue>> PostVenueFilter([FromBody] Venue venueLike)
        {
            if (venueLike == null)
            {
                BadRequest();
                return null;
            }
            var venues = (await _venuesDbRepository.Filter(venueLike)).Take(10).ToList();
            if (venues == null)
            {
                NotFound();
                return null;
            }
            else
                Ok();
            return from _ in venues
                   select _.UnFixLoops();
        }
    }
}