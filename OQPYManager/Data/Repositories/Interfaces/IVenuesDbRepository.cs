using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OQPYManager.Data.Interface
{
    public interface IVenuesDbRepository
    {
        Task AddVenueAsync(Venue venue);

        Task AddVenuesAsync(params Venue[] venues);

        Task AddVenuesAsync(IEnumerable<Venue> venues);

        IEnumerable<Venue> GetAllVenues();

        /// <summary>
        /// Get all venues with included parameters
        /// </summary>
        /// <param name="includedParams">list of included parameters e.g. "Resources"</param>
        /// <returns>list of venues with included parameters</returns>
        IEnumerable<Venue> GetAllVenues(params string[] includedParams);

        /// <summary>
        /// Get all venues with included parameters
        /// </summary>
        /// <param name="includedParams">list of included parameters seperated with ';' e.g. "Resources;Resources.Reservations"</param>
        /// <returns>list of venues with included parameters</returns>
        IEnumerable<Venue> GetAllVenues(string includedParams);

        /// <summary>
        /// Gets venues based on passed filters.
        /// </summary>
        /// <param name="filters">Filters a venue must pass.</param>
        /// <returns>Venues that passed all filters.</returns>
        IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters);

        /// <summary>
        /// Gets venues based on passed filters.
        /// </summary>
        /// <param name="includedParams"></param>
        /// <param name="filters">Filters a venue must pass.</param>
        /// <returns>Venues that passed all filters.</returns>
        IEnumerable<Venue> GetVenues(string includedParams, params Func<Venue, bool>[] filters);

        /// <summary>
        /// Finds venue based on key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Venue with the key or null.</returns>
        Task<Venue> FindVenueAsync(string key);

        /// <summary>
        /// Removes venue from database which has passed key.
        /// </summary>
        /// <param name="key">Passed key.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Update a venue in database with new values.
        /// </summary>
        /// <param name="venue">Venue which holds new values.</param>
        Task UpdateAsync(Venue venue);
    }
}