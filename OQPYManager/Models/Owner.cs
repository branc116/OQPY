using OQPYManager.Models.CoreModels;
using System.Collections.Generic;

namespace OQPYManager.Models
{
    /// <summary>
    /// I created this class in order to simplify database creation.
    /// </summary>
    public class Owner : ApplicationUser
    {
        /// <summary>
        /// Owner can own multiple venues.
        /// </summary>
        public List<Venue> Venues { get; set; }
    }
}