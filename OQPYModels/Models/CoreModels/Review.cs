using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// This class is used to describe reviewing a venue.
    /// It holds comment and numerical rating as core feautures.
    /// I think it should hold some userdata about reviewer so we can prevent false
    /// reviews and comments.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Text of a review
        /// </summary>
        public string Comment { get; set; }

        public string Id { get; set; }

        private int _rating;

        /// <summary>
        /// Venue to be reviewed
        /// </summary>
        public virtual Venue Venue { get; set; }

        /// <summary>
        /// Somewhat a numerical representation of a review, a summary
        /// Ensuring the rating wont go below minimum or exceed maximum.
        /// </summary>
        public int Rating
        {
            get { return _rating; }
            set
            {
                if ( value < Constants.MinimumRating )
                    _rating = Constants.MinimumRating;
                else if ( value > Constants.MaximumRating )
                    _rating = Constants.MaximumRating;
                else
                    _rating = value;
            }
        }

        /// <summary>
        /// Keep track of likes and dislikes on this Review
        /// </summary>
        public int Helpfulness { get; set; }

        public Review()
        {
        }

        public Review(int rating, string comment)
        {
            Id = Guid.NewGuid().ToString();
            this.Rating = rating;
            this.Comment = comment;
        }

        public Review(int rating, string comment, Venue venue)
        {
            Id = Guid.NewGuid().ToString();
            this.Rating = rating;
            this.Comment = comment;
            this.Venue = venue;
        }

        public static IEnumerable<Review> RandomReviews(int n, Venue venue)
        {
            return from _ in new string('a', n)
                   let text = RandomText(50, 100)
                   let review = RandomInt(Constants.MinimumRating, Constants.MaximumRating)
                   select new Review(review, text, venue);
        }
    }
}