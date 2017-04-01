using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BaseReview
    {
        /// <summary>
        /// Text of a review
        /// </summary>
        public virtual string Comment { get; set; }

        public virtual string Id { get; set; }

        private int _rating;

        /// <summary>
        /// Venue to be reviewed
        /// </summary>
        public virtual BaseVenue Venue { get; set; }

        /// <summary>
        /// Somewhat a numerical representation of a review, a summary
        /// Ensuring the rating wont go below minimum or exceed maximum.
        /// </summary>
        public virtual int Rating
        {
            get { return _rating; }
            set
            {
                if (value < Constants.MinimumRating)
                    _rating = Constants.MinimumRating;
                else if (value > Constants.MaximumRating)
                    _rating = Constants.MaximumRating;
                else
                    _rating = value;
            }
        }

        public BaseReview(int rating, string comment)
        {
            Id = Guid.NewGuid().ToString();
            this.Rating = rating;
            this.Comment = comment;
        }
        public BaseReview(int rating, string comment, BaseVenue venue)
        {
            Id = Guid.NewGuid().ToString();
            this.Rating = rating;
            this.Comment = comment;
            this.Venue = venue;
        }

        public static IEnumerable<BaseReview> RandomReviews(int n, BaseVenue venue)
        {
            return from _ in new string('a', n)
                   let text = RandomText(50, 100)
                   let review = RandomInt(Constants.MinimumRating, Constants.MaximumRating)
                   select new BaseReview(review, text, venue);
        }
    }
}