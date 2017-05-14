using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace OQPYModels.Models.CoreModels
{
    public class BaseApplicationUser: IdentityUser
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        
        public BaseApplicationUser(string userName) : base(userName)
        {
        }

        public BaseApplicationUser() : base()
        {
        }
    }
}