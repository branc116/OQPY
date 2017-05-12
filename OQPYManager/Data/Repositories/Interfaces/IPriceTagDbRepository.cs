using System.Collections.Generic;
using System.Threading.Tasks;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IPriceTagDbRepository : IBaseDbRepository<PriceTag>
    {
        ///// <summary>
        ///// Change price for an item.
        ///// </summary>
        ///// <param name="price"></param>
        //void ChangePrice(decimal price);

        /// <summary>
        /// Shows prices for venue.
        /// </summary>
        /// <param name="venueId">Id of the venue</param>
        /// <returns></returns>
        Task<IEnumerable<PriceTag>> GetPriceTagsForVenue(string venueId);


    }
}