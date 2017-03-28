using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Models.CoreModels;

namespace OQPYManager.Models
{
    public class Employee : ApplicationUser
    {
        /// <summary>
        /// Venue where worker works.
        /// Should we put multiple venues if he works in multiple venues?
        /// note : very unlikely situation altough possible
        /// </summary>
        public Venue Venue { get; set; }
    }
}
