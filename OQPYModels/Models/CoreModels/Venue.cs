using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYModels.Models.CoreModels
{
    public class BaseVenue
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        ///Connect the venue to its owner
        /// </summary>
        public virtual IApplicationUser Owner { get; set; }

        /// <summary>
        /// Always useful to know when the venue was signed up on our service.
        /// </summary>
        public DateTime VenueCreationDate { get; set; }

        /// <summary>
        ///Data essential for locating a venue
        ///Possibly compactible with Google Maps, get its data directly from Google.
        /// </summary>
        public virtual BaseLocation Location { get; set; }

        /// <summary>
        ///All the occupable objects inside hospitality venues 
        /// </summary>
        public virtual List<BaseResource> Resources{ get; set; }

        /// <summary>
        ///List of tags define which features a venue has
        ///e.g. pool table 
        /// </summary>
        public virtual List<BaseTag> Tags { get; set; }

        /// <summary>
        ///Users share their opinion about a particular venue
        ///e.g. 5/5 a great venue!
        /// </summary> 
        // Better name than rating
        public virtual List<BaseReview> Reviews { get; set; }

        /// <summary>
        /// Text that describes venue.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// List of prices or various merchandises and services of a venue.
        /// </summary>
        public virtual List<BasePriceTag> PriceTags { get; set; }

        /// <summary>
        /// Property which will define when a venue works.
        /// The type will also support changing status of working manually
        /// if e.g. sudden inspection, deratization etc.
        /// </summary>
        public virtual BaseWorkHours WorkHours{ get; set; }

        /// <summary>
        /// List of employees working in a venue, excluding owner.
        /// </summary>
        public virtual List<IApplicationUser> Employees { get; set; }

        /// <summary>
        /// Enables many-to-many relationship with tags.
        /// </summary>
        public virtual List<BaseVenueTag> VenueTags { get; set; }

        public string ImageUrl { get; set; }
    }
}
