using System;
using System.Collections.Generic;
using System.Linq;
using OQPYModels.Helper;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    public class Venue : ICoreModel<Venue>
    {
        public Venue()
        {
        }

        public Venue(string name, string ownerUsername, string imageUrl, string location)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Owner = new Owner(ownerUsername, this);
            ImageUrl = imageUrl;
            VenueCreationDate = DateTime.Now;
            Location = new Location {Id = Guid.NewGuid().ToString(), Adress = location};
        }

        public Venue(string name, string ownerUsername, string imageUrl, string location, string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
            Name = name;
            //Owner = new Owner(ownerUsername, this);
            ImageUrl = imageUrl;
            VenueCreationDate = DateTime.Now;
            Location = new Location {Id = Guid.NewGuid().ToString(), Adress = location};
        }

        public virtual string Id { get; set; }

        [Filterable(Filter = true)]
        public virtual string Name { get; set; }

        /// <summary>
        ///     Connect the venue to its owner
        /// </summary>
        public virtual Owner Owner { get; set; }

        /// <summary>
        ///     Always useful to know when the venue was signed up on our service.
        /// </summary>
        [Filterable(Filter = true)]
        public DateTime VenueCreationDate { get; set; }

        /// <summary>
        ///     Data essential for locating a venue
        ///     Possibly compactible with Google Maps, get its data directly from Google.
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        ///     All the occupable objects inside hospitality venues
        /// </summary>
        [Filterable(Filter = true, IsList = true)]
        public virtual List<Resource> Resources { get; set; }

        /// <summary>
        ///     List of tags define which features a venue has
        ///     e.g. pool table
        /// </summary>
        [Filterable(Filter = true, IsList = true)]
        public virtual List<Tag> Tags { get; set; }

        /// <summary>
        ///     Users share their opinion about a particular venue
        ///     e.g. 5/5 a great venue!
        /// </summary>
        // Better name than rating
        [Filterable(Filter = true, IsList = true)]
        public virtual List<Review> Reviews { get; set; }

        /// <summary>
        ///     Text that describes venue.
        /// </summary>
        [Filterable(Filter = true)]
        public virtual string Description { get; set; }

        /// <summary>
        ///     List of prices or various merchandises and services of a venue.
        /// </summary>
        public virtual List<PriceTag> PriceTags { get; set; }

        /// <summary>
        ///     Property which will define when a venue works.
        ///     The type will also support changing status of working manually
        ///     if e.g. sudden inspection, deratization etc.
        /// </summary>

        public virtual WorkHours WorkHours { get; set; }

        /// <summary>
        ///     List of employees working in a venue, excluding owner.
        /// </summary>
        [Filterable(Filter = true, IsList = true)]
        public virtual List<Employee> Employees { get; set; }

        /// <summary>
        ///     Enables many-to-many relationship with tags.
        /// </summary>
        [Filterable(Filter = true, IsList = true)]
        public virtual List<VenueTag> VenueTags { get; set; }

        public string ImageUrl { get; set; }

        [Filterable(Filter = true)]
        public decimal AverageReview => Math.Round(
            (Reviews?.Select(i => i?.Rating)?.Sum() ?? -1) / (decimal) ((Reviews?.Count ?? 1) + 0.1), 1);

        public IEnumerable<Reservation> Reservations => Resources == null
            ? null
            : from _ in Resources
            where _.Reservations != null
            from __ in _.Reservations
            select __;

        public bool Filter(Venue one, Venue two)
        {
            return one != null && two != null &&
                   (one.Name == null || two.Name == null || one.Name.ToLower().Contains(two.Name.ToLower()) ||
                    two.Name.ToLower().Contains(one.Name.ToLower())) &&
                   (one.AverageReview == -1 || two.AverageReview == -1 ||
                    Math.Abs(two.AverageReview - one.AverageReview) < 3) &&
                   (one.Description == null || two.Description == null ||
                    one.Description.ToLower().Contains(two.Description.ToLower()) ||
                    two.Description.ToLower().Contains(one.Description.ToLower())) &&
                   (one.Location == null && two.Location == null ||
                    one != null && one.Location.Filter(one.Location, two.Location) ||
                    one != null && one.Location.Filter(one.Location, two.Location));
        }

        public Venue FixLoops()
        {
            if (Employees != null)
                foreach (var emp in Employees)
                    emp.Venue = this;
            if (PriceTags != null)
                foreach (var _ in PriceTags)
                    _.Venue = this;
            if (Resources != null)
                foreach (var _ in Resources)
                    _.Venue = this;
            if (Reviews != null)
                foreach (var _ in Reviews)
                    _.Venue = this;
            if (WorkHours != null)
            {
                WorkHours.Venue = this;
                WorkHours.FixLoops();
            }
            return this;
        }

        public Venue UnFixLoops()
        {
            if (Employees != null)
                foreach (var emp in Employees)
                    emp.Venue = null;
            if (PriceTags != null)
                foreach (var _ in PriceTags)
                    _.Venue = null;
            if (Resources != null)
                foreach (var _ in Resources)
                    _.Venue = null;
            if (Reviews != null)
                foreach (var _ in Reviews)
                    _.Venue = null;
            if (WorkHours != null)
            {
                WorkHours.Venue = null;
                WorkHours.UnFixLoops();
            }
            if (Tags != null)
                foreach (var t in Tags)
                    t.UnFixLoops();
            VenueTags = null;
            return this;
        }

        public static IEnumerable<Venue> CreateRandomVenues(int n)
        {
            return from _ in new string(' ', n)
                let rand = new Random()
                let names = RandomName()
                let images = RandomUriOfVenue()
                let location = RandomName()
                let ownerUserName = RandomName()
                let discription = RandomText(40, 50)
                let venue = new Venue(names, ownerUserName, images, location)
                {
                    Description = discription
                }
                let empleys = venue.Employees = Employee.RandomEmployees(rand.Next(1, 4), venue).ToList()
                let pricetags = venue.PriceTags = PriceTag.RandomPriceTags(rand.Next(10, 20), venue).ToList()
                let resouces = venue.Resources = Resource.RandomResources(rand.Next(5, 10), venue).ToList()
                let reviews = venue.Reviews = Review.RandomReviews(rand.Next(5, 10), venue).ToList()
                let tags = venue.Tags = Tag.RandomTags(rand.Next(5, 10), venue).ToList()
                let workHours = venue.WorkHours = WorkHours.RandomWorkHours(1, venue).FirstOrDefault()
                select venue;
        }

        public override string ToString()
        {
            var s =
                $"Venue.Name = {Name}\nVenue.Location = {this?.Location?.Adress ?? ""}, ({this?.Location?.Longditude ?? 0}, {this?.Location?.Latitude ?? 0})\n";
            return s;
        }
    }
}