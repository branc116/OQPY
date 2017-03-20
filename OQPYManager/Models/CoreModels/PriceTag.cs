namespace OQPYManager.Models.CoreModels
{
    public class PriceTag
    {
        public string Id { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public Venue Venue { get; set; }


    }
}