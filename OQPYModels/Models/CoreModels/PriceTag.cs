namespace OQPYModels.Models.CoreModels
{
    public class BasePriceTag
    {
        public virtual string Id { get; set; }

        public virtual string ItemName { get; set; }

        public virtual decimal Price { get; set; }

        public virtual BaseVenue Venue { get; set; }
    }
}