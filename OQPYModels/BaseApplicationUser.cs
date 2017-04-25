using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OQPYModels
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