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
    }
}