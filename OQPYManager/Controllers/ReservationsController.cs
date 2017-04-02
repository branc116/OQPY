using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Extensions;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYModels.Models.CoreModels.ErrorMessages;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Reservations")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public IEnumerable<Reservation> GetReservations()
        {
            return _context.Reservations;
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.Id == id);

            if (Reservation == null)
            {
                return NotFound();
            }

            return Ok(Reservation);
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation([FromRoute] string id, [FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        [HttpPost]
        public async Task<IActionResult> PostReservation([FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.Id == id);
            if (Reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(Reservation);
            await _context.SaveChangesAsync();

            return Ok(Reservation);
        }

        private bool ReservationExists(string id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        [Route("VenueReservation")]
        [HttpGet]
        public IEnumerable<Reservation> GetReservationFromVenues([FromHeader] string venueId)
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
        public IEnumerable<Reservation> GetReservationFromResource([FromHeader] string resourceId)
        {
            return (from _ in _context.Resources
                   .Where(i => i.Id == resourceId)
                   ?.Include(i => i.Reservations)
                    let reserv = _.Reservations
                    select reserv)?.ToList()?.Aggregate((i, j) => { i.AddRange(j); return i; }) ?? null;
        }

        [Route("SecretCodeReservation")]
        [HttpGet]
        public async Task<Reservation> GetReservationFromSecretCode([FromHeader] string secretCode)
        {
            return await _context.Reservations
                .Where(i => i.SecretCode == secretCode)
                .Include(i => i.Resource)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        [Route("ResourceReservation")]
        public async Task<IActionResult> PostReservationToResource([FromHeader] string resourceId, [FromBody] DateTime from, [FromBody] DateTime to)
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

            var reservation = new Reservation(from, to, resource);
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id });
        }

        [HttpDelete]
        [Route("ResourceReservation")]
        public async Task<IActionResult> DeleteReservation([FromHeader] string resourceId, [FromBody] DateTime from, [FromBody] DateTime to)
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