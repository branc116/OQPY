using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Models.CoreModels
{
    public class Venue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///Connect the venue to its owner
        /// </summary>
        public ApplicationUser Owner { get; set; }

        /// <summary>
        ///Data essential for locating a venue
        ///Possibly compactible with Google Maps, get its data directly from Google.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        ///All the occupable objects inside hospitality venues 
        /// </summary>
        public List<Resource> Resources{ get; set; }

        /// <summary>
        ///List of tags define which features a venue has
        ///e.g. pool table 
        /// </summary>
        public List<Tag> Tags { get; set; }

        /// <summary>
        ///Users share their opinion about a particular venue
        ///e.g. 5/5 a great venue!
        /// </summary> 
        // Better name than rating
        public List<Review> Reviews { get; set; }

        /// <summary>
        /// Text that describes venue.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of prices or various merchandises and services of a venue.
        /// </summary>
        public List<PriceTag> PriceTags { get; set; }

        /// <summary>
        /// Property which will define when a venue works.
        /// The type will also support changing status of working manually
        /// if e.g. sudden inspection, deratization etc.
        /// </summary>
        public WorkHours WorkHours{ get; set; }

        /// <summary>
        /// List of employees working in a venue, excluding owner.
        /// </summary>
        public List<ApplicationUser> Employees { get; set; }

        /// <summary>
        /// Enables many-to-many relationship with tags.
        /// </summary>
        public List<VenueTag> VenueTags { get; set; }

    }
}
