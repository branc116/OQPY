using OQPYModels.Models.CoreModels;

namespace OQPYManager.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : BaseApplicationUser
    {
        public ApplicationUser(string userName) : base(userName)
        {
        }

        public ApplicationUser() : base()
        {
        }
    }
}