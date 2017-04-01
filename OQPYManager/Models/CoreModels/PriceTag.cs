using OQPYModels.Models.CoreModels;

namespace OQPYManager.Models.CoreModels
{
    public class PriceTag : BasePriceTag
    {
        public PriceTag(string itemName, decimal price, BaseVenue venue) : base(itemName, price, venue)
        {

        }
    }
}