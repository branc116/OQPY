using System;
using System.Collections.Generic;
using System.Linq;
using static OQPYModels.Helper.Helper;
namespace OQPYModels.Models.CoreModels
{
    public class BasePriceTag
    {
        public virtual string Id { get; set; }

        public virtual string ItemName { get; set; }

        public virtual decimal Price { get; set; }

        public virtual BaseVenue Venue { get; set; }

        public BasePriceTag(string itemName, decimal price)
        {
            this.ItemName = itemName;
            this.Price = price;
            this.Id = Guid.NewGuid().ToString();
        }
        public BasePriceTag(string itemName, decimal price, BaseVenue venue)
        {
            this.ItemName = itemName;
            this.Price = price;
            Venue = venue;
            this.Id = Guid.NewGuid().ToString();
        }
        public static IEnumerable<BasePriceTag> RandomPriceTags(int n, BaseVenue venue)
        {
            return from _ in new string(' ', n)
                   let price = RandomDecimal(20, 100)
                   let name = RandomName()
                   select new BasePriceTag(name, price, venue);

        }
    }
}