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


    }
}