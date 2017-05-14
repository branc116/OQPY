using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OQPYHelper.AuthHelper.Auth;

namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Resources")]
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourceDbRepository _dbContext;

        public ResourcesController(ApplicationDbContext context,
                                   IResourceDbRepository dbContext)
        {
            _dbContext = dbContext;
            _context = context;
        }

        // GET: api/Resources
        [HttpGet]
        public IEnumerable<Resource> GetResources([FromHeader] string masterAdminKey)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                Ok();
                return _context.Resources;
            }
            Unauthorized();
            return null;
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resource = await _dbContext.FindAsync(id);
            return Ok(resource);
        }

        [HttpPost]
        [Route("IOT")]
        public async Task UpdateStatus([FromHeader] string id, [FromHeader] string OQPYed, [FromHeader] string secretCode)
        {
            await _dbContext.ChangeState(id, OQPYed != "0", secretCode);
        }
    }
}