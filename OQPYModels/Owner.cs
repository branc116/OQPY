using OQPYModels.Models.CoreModels;
using System.Collections.Generic;

namespace OQPYModels.Models
{
    /// <summary>
    /// I created this class in order to simplify data creation.
    /// </summary>
    public class Owner : BaseApplicationUser
    {
        /// <summary>
        /// Owner can own multiple venues.
        /// </summary>
        public virtual List<Venue> Venues { get; set; }

        public Owner(string userName) : base(userName)
        {
        }

        public Owner(string userName, Venue venue) : base(userName)
        {
            Venues = new List<Venue>() { venue };
        }

        //public Owner() : ()
        //{
        //}
    }
}