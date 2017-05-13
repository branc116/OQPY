using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// PriceTag describes an item or service in a venue and what its price is.
    /// </summary>
    public class PriceTag
    {
        public virtual string Id { get; set; }

        public virtual string ItemName { get; set; }

        public virtual decimal Price { get; set; }

        public virtual Venue Venue { get; set; }

        public PriceTag()
        {
        }

        public PriceTag(string itemName, decimal price)
        {
            this.ItemName = itemName;
            this.Price = price;
            this.Id = Guid.NewGuid().ToString();
        }

        public PriceTag(string itemName, decimal price, Venue venue)
        {
            this.ItemName = itemName;
            this.Price = price;
            Venue = venue;
            this.Id = Guid.NewGuid().ToString();
        }

        public static IEnumerable<PriceTag> RandomPriceTags(int n, Venue venue)
        {
            return from _ in new string(' ', n)
                   let price = RandomDecimal(20, 100)
                   let name = RandomName()
                   select new PriceTag(name, price, venue);
        }
    }
}