using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using OQPYModels.Extensions;
using static OQPYModels.Models.CoreModels.ErrorMessages;
namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/BaseReservations")]
    public class BaseReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BaseReservations
        [HttpGet]
        public IEnumerable<BaseReservation> GetReservations()
        {
            return _context.Reservations;
        }

        // GET: api/BaseReservations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBaseReservation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseReservation = await _context.Reservations.SingleOrDefaultAsync(m => m.Id == id);

            if (baseReservation == null)
            {
                return NotFound();
            }

            return Ok(baseReservation);
        }

        // PUT: api/BaseReservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBaseReservation([FromRoute] string id, [FromBody] BaseReservation baseReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != baseReservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(baseReservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaseReservationExists(id))
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

        // POST: api/BaseReservations
        [HttpPost]
        public async Task<IActionResult> PostBaseReservation([FromBody] BaseReservation baseReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _context.Reservations.Add(baseReservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseReservation", new { id = baseReservation.Id }, baseReservation);
        }

        // DELETE: api/BaseReservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaseReservation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baseReservation = await _context.Reservations.SingleOrDefaultAsync(m => m.Id == id);
            if (baseReservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(baseReservation);
            await _context.SaveChangesAsync();

            return Ok(baseReservation);
        }

        private bool BaseReservationExists(string id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        [Route("VenueReservation")]
        [HttpGet]
        public IEnumerable<BaseReservation> GetBaseReservationFromVenues([FromHeader] string venueId)
        {
            return (from _ in _context.Resources
                   .Include(i => i.Venue)
                   .Where(i => i.Venue.Id == venueId)
                   ?.Include(i => i.Reservations)
                    let reserv = _.Reservations
                    select reserv)?.ToList()?.Aggregate((i, j) => { i.AddRange(j); return i; }) ?? null;
        }
        [Route("ResourceReservation")]
        [HttpGet]
        public IEnumerable<BaseReservation> GetBaseReservationFromResource([FromHeader] string resourceId)
        {
            return (from _ in _context.Resources
                   .Where(i => i.Id == resourceId)
                   ?.Include(i => i.Reservations)
                    let reserv = _.Reservations
                    select reserv)?.ToList()?.Aggregate((i, j) => { i.AddRange(j); return i; }) ?? null;
        }
        [Route("SecretCodeReservation")]
        [HttpGet]
        public async Task<BaseReservation> GetBaseReservationFromSecretCode([FromHeader] string secretCode)
        {
            return await _context.Reservations
                .Where(i => i.SecretCode == secretCode)
                .Include(i => i.Resource)
                .FirstOrDefaultAsync();
        }
        [HttpPost]
        [Route("ResourceReservation")]
        public async Task<IActionResult> PostBaseReservaationToResource([FromHeader] string resourceId, [FromHeader] DateTime from, [FromHeader] DateTime to)
        {
            if ((to - from).TotalDays < 1)
                return BadRequest(new { error = Reservation2Long });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _context.Resources
                .Where(i => i.Id == resourceId)
                .Include(i => i.Reservations)
                .Include(i => i.Venue)
                .FirstOrDefaultAsync();
            if (resource == null)
                return NotFound(new { resourceId = resourceId });

            var taken = resource.Reservable(from, to);
            if (taken)
                return BadRequest(new { error = ReservatioAlredyTaken });

            var workTimes = await _context.VenueWorkHours
                .Where(i => i.VenueId == resource.Venue.Id)
                .Include(i => i.WorkTimes)
                .FirstOrDefaultAsync();
            var working = workTimes.Working(from, to);
            if (!working)
                return BadRequest(new { error = ClosedInThisTime });

            var reservation = new BaseReservation(from, to, resource);
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseReservation", new { id = reservation.Id });
        }
        [HttpDelete]
        [Route("ResourceReservation")]
        public async Task<IActionResult> DeleteBaseReservation([FromHeader] string resourceId, [FromHeader] DateTime from, [FromHeader] DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resource = await _context.Resources
                .Where(i => i.Id == resourceId)
                .Include(i => i.Reservations)
                .FirstOrDefaultAsync();
            if (resource == null)
                return NotFound(new { Resourceid = resourceId });
            var n = resource.Reservations.RemoveAll(i => i.StartReservationTime > from && i.StartReservationTime < to);
            await _context.SaveChangesAsync();

            return Ok(new { n = n });
        }
    }
}