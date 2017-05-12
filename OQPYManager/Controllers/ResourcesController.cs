using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Resources")]
    public class ResourcesController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourceDbRepository _resourceDbRepository;

        public ResourcesController(ApplicationDbContext context,
                                   IResourceDbRepository dbContext)
        {
            _resourceDbRepository = dbContext;
            _context = context;
        }

        // GET: api/Resources
        [HttpGet]
        public IEnumerable<Resource> GetResources()
        {
            return _resourceDbRepository.GetAll();
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource([FromRoute] string id)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            var resource = await _resourceDbRepository.FindAsync(id);
            return Ok(resource);
        }

        // PUT: api/Resources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource([FromRoute] string id, [FromBody] Resource resource)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            if ( id != resource.Id )
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch ( DbUpdateConcurrencyException )
            {
                if ( !ResourceExists(id) )
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

        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> PostResource([FromBody] Resource resource)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource([FromRoute] string id)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            

            //_context.Resources.Remove(resource);
            await _resourceDbRepository.RemoveAsync(id);
            

            return Ok(id);
        }

        private bool ResourceExists(string id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("IOT")]
        public async Task UpdateStatus([FromHeader] string id, [FromHeader] string OQPYed, [FromHeader] string secretCode)
        {
            await _resourceDbRepository.ChangeState(id, OQPYed != "0", secretCode);
        }
    }
}