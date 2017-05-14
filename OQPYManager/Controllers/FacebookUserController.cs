using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data;
using OQPYManager.Models;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OQPYHelper.AuthHelper.Auth;
namespace OQPYManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class FacebookUserController : Controller
    {
        private ApplicationDbContext _context;

        public FacebookUserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("All")]
        public IEnumerable<FacebookUser> GetAllUsers([FromHeader] string MasterAdminKey)
        {
            if (ValidateMasterAdminKey(MasterAdminKey))
            {
                Ok();
                return _context.FacebookUsers;
            }
            Unauthorized();
            return null;
        }
        /// <summary>
        /// Add now user from faceook auth
        /// </summary>
        /// <param name="facebookToken">Facebook token</param>
        /// <returns>true if everything was ok</returns>
        [HttpPost]
        public async Task<bool> PostUser([FromHeader] string facebookToken)
        {
            if (await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(facebookToken))
            {
                var fbUser = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(facebookToken);
                FacebookUser user = _context.FacebookUsers.FirstOrDefault(i => i.Id == fbUser.Id);
                if (user == null)
                {
                    await _context.FacebookUsers.AddAsync(new FacebookUser() { Id = fbUser?.Id ?? "deffaoult", Name = fbUser?.Name ?? "Undeff"});
                    await _context.SaveChangesAsync();
                    Ok(true);
                }
                else
                {
                    return await UpdateToken(facebookToken);
                }
                return true;
            }
            Unauthorized();
            return false;
        }
        [HttpGet]
        public async Task<FacebookUser> GetUser([FromHeader] string MasterAdminKey, [FromHeader] string userId)
        {
            if (ValidateMasterAdminKey(MasterAdminKey))
            {
                var user = _context.FacebookUsers.FirstOrDefault(i => i.Id == userId);
                if (user != null){
                    return user;
                }
                else
                {
                    NotFound(nameof(userId));
                    return null;
                }
                
            }
            Unauthorized();
            return null;
        } 
        [HttpGet]
        [Route("exists")]
        public async Task<bool> ExistsUser([FromHeader] string masterAdminKey, [FromHeader] string userId)
        {
            if (ValidateMasterAdminKey(masterAdminKey))
            {
                var exists = _context.FacebookUsers.Any(i => i.Id == userId);
                return exists;
            }

            Unauthorized();
            return false;
        }
        [HttpPut]
        public async Task<bool> UpdateToken([FromHeader] string newFacebookToken)
        {
            if (await OQPYHelper.AuthHelper.FacebookHelpers.ValidateAccessToken(newFacebookToken))
            {
                var fbUser = await OQPYHelper.AuthHelper.FacebookHelpers.GetFacebookProfile(newFacebookToken);
                var user = _context.FacebookUsers.FirstOrDefault(i => i.Id == fbUser.Id);
                if (user != null)
                {
                    user.Name = fbUser.Name;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    NotFound(nameof(newFacebookToken));
                    return false;
                }
            }
            Unauthorized();
            return false;
        }
    }
}
