using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Models.CoreModels;

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
