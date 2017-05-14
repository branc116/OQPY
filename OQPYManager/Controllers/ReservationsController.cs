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
using static OQPYHelper.AuthHelper.Auth;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Reservations")]
    public class ReservationsController: Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        [Route("All")]
        public IEnumerable<Reservation> GetReservations([FromHeader] string masterAdminKey)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                Ok();
                return _context.Reservations;
            }
            Unauthorized();
            return null;
        }

        // GET: api/Reservations/5
        [HttpGet]
        public async Task<Reservation> GetReservation([FromHeader] string id, [FromHeader] string facebookAuth)
        {
            if ( !ModelState.IsValid )
            {
                BadRequest(ModelState);
                return null;
            }

            var reservation = await _context.Reservations
                .Include(i => i.FacebookUsers)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (reservation == null)
                NotFound(id);

            if (await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(facebookAuth))
            {
                var user = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(facebookAuth);
                if (reservation.FacebookUsers.Id != user.Id)
                {
                    reservation.SecretCode = string.Empty;
                }
            }
            else
            {
                reservation.SecretCode = string.Empty;
            }
            ;
            reservation.FacebookUsers= null;
            await _context.SaveChangesAsync();
            Ok();
            return reservation;
        }
        
        // DELETE: api/Reservations/5
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation([FromHeader] string id, [FromHeader] string facebookAuth, [FromHeader] string masterAdminKey )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            if (!_context.Reservations.Any(i => id == i.Id))
                return NotFound();
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                await removeForReal();
                return Ok();
            }
            if (await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(facebookAuth))
            {

                var userDb = await _context.Reservations
                    .Include(i => i.FacebookUsers)
                    .Where(i => i.Id == id)
                    .Select(i => i.FacebookUsers.Id)
                    .FirstOrDefaultAsync();
                var fbUser = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(facebookAuth);
                if (userDb == fbUser.Id)
                {
                    await removeForReal();
                    return Ok();
                }
            }
            return Unauthorized();
            
            async Task removeForReal()
            {
                var res = await _context.Reservations.FirstOrDefaultAsync(i => i.Id == id);
                _context.Reservations.Remove(res);
                await _context.SaveChangesAsync();
                
            }
        }

        [Route("VenueReservation")]
        [HttpGet]
        public IEnumerable<Reservation> GetReservationFromVenues([FromHeader] string venueId)
        {
            if ( !ModelState.IsValid )
            {
                base.BadRequest();
                return null;
            }
            var ret = (from _ in _context.Resources
                                         .Include(i => i.Venue)
                                         .Where(i => i.Venue.Id == venueId)
                                         ?.Include(i => i.Reservations)
                       let reserv = _.Reservations
                       select reserv)?.ToList()?.Aggregate((i, j) => { i.AddRange(j); return i; }) ?? null;
            if ( ret == null )
                base.NotFound();
            else
                base.Ok();
            return ret;
        }

        [Route("ResourceReservation")]
        [HttpGet]
        public IEnumerable<Reservation> GetReservationFromResource([FromHeader] string resourceId)
        {
            if ( !ModelState.IsValid )
            {
                base.BadRequest();
                return null;
            }
            var ret = (from _ in _context.Resources
                                         .Where(i => i.Id == resourceId)
                                         ?.Include(i => i.Reservations)
                       let reserv = _.Reservations
                       select reserv)?.ToList()?.Aggregate((i, j) => { i.AddRange(j); return i; }) ?? null;
            if ( ret == null )
                base.NotFound();
            else
                base.Ok();
            return ret;
        }

        [Route("SecretCodeReservation")]
        [HttpGet]
        public async Task<Reservation> GetReservationFromSecretCode([FromHeader] string secretCode)
        {
            if ( !ModelState.IsValid )
            {
                base.BadRequest();
                return null;
            }
            var ret = await _context.Reservations
                                    .Where(i => i.SecretCode == secretCode)
                                    .Include(i => i.Resource)
                                    .FirstOrDefaultAsync();
            if ( ret == null )
                base.NotFound();
            else
                base.Ok();
            return ret;
        }

        [HttpPost]
        [Route("ResourceReservation")]
        public async Task<IActionResult> PostReservationToResource([FromHeader] string resourceId, [FromHeader] string facebookAuth, [FromBody] DateTime from, [FromBody] DateTime to)
        {
            if (!await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(facebookAuth))
                return Unauthorized();
            var user = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(facebookAuth);
            var userDb = await _context.FacebookUsers
                .Include(i => i.Reservations)
                .FirstOrDefaultAsync(i => i.Id != user.Id);
            if (userDb == null)
            {
                return Unauthorized();
            }
            if ( (to - from).TotalDays < 1 )
                return BadRequest(new { error = Reservation2Long });

            if ( !ModelState.IsValid )
                return BadRequest(ModelState);

            var resource = await _context.Resources
                .Where(i => i.Id == resourceId)
                .Include(i => i.Reservations)
                .Include(i => i.Venue)
                .FirstOrDefaultAsync();

            if ( resource == null )
                return NotFound(new { resourceId = resourceId });

            var taken = resource.Reservable(from, to);
            if ( taken )
                return BadRequest(new { error = ReservatioAlredyTaken });

            var workTimes = await _context.VenueWorkHours
                .Where(i => i.VenueId == resource.Venue.Id)
                .Include(i => i.WorkTimes)
                .FirstOrDefaultAsync();
            var working = workTimes.Working(from, to);
            if ( !working )
                return BadRequest(new { error = ClosedInThisTime });

            
            var reservation = new Reservation(from, to, resource)
            {
                FacebookUsers = userDb
            };
            await _context.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("ResourceReservation")]
        public async Task<IActionResult> DeleteReservation([FromHeader] string resourceId, [FromHeader] string masterAdminKey, [FromBody] DateTime from, [FromBody] DateTime to)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
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

                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("My")]
        public async Task<IEnumerable<Reservation>> GetMyReservations([FromHeader] string facebookAuth)
        {
            if (!await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(facebookAuth))
            {
                Unauthorized();
                return null;
            }
            var user = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(facebookAuth);
            return _context.Reservations
                .Include(i => i.FacebookUsers)
                .Include(i => i.Resource)
                .OrderBy(i => i.StartReservationTime)
                .Where(i => i.FacebookUsers.Id == user.Id);
        }
    }
}