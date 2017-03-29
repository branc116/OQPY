using OQPYModels.Models.CoreModels;

namespace OQPYModels.Models
{
    public class BaseEmployee : BaseApplicationUser
    {
        /// <summary>
        /// Venue where worker works.
        /// Should we put multiple venues if he works in multiple venues?
        /// note : very unlikely situation altough possible
        /// Better List<Venue>, what if he is a manager, but not the owner and he is managing more venues
        /// </summary>
        public virtual BaseVenue Venue { get; set; }
    }
}