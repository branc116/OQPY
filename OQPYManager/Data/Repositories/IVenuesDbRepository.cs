using System;
using System.Collections.Generic;
using System.Linq;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data
{
    public interface IVenuesDbRepository
    {
        void AddVenue(Venue venue);

        IEnumerable<Venue> GetAllVenues();

            /// <summary>
        /// Gets venues based on passed filters.
        /// </summary>
        /// <param name="filters">Filters a venue must pass.</param>
        /// <returns>Venues that passed all filters.</returns>
        IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters);

        /// <summary>
        /// Finds venue based on key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Venue with the key or null.</returns>
        Venue FindVenue(string key);

        /// <summary>
        /// Removes venue from database which has passed key.
        /// </summary>
        /// <param name="key">Passed key.</param>
        void Remove(string key);

        /// <summary>
        /// Update a venue in database with new values.
        /// </summary>
        /// <param name="venue">Venue which holds new values.</param>
        void Update(Venue venue);
    }
}
