using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OQPYModels;

namespace OQPYManager.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}