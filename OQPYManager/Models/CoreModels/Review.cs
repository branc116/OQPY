namespace OQPYManager.Models.CoreModels
{
    public class Review
    {
        
        public string Comment { get; set; }

        public string Id { get; set; }

        private int _rating;

        public int Rating
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