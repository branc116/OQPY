using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IBaseDbRepository<T>: IBaseContext where T : ICoreModel<T>
    {
        Task OnCreate();

        Task AddAsync(T item);

        Task AddAsync(params T[] items);

        Task AddAsync(IEnumerable<T> items);

        IEnumerable<T> GetAll();

        /// <summary>
        /// Get all venues with included parameters
        /// </summary>
        /// <param name="includedParams">list of included parameters e.g. "Resources"</param>
        /// <returns>list of venues with included parameters</returns>
        IEnumerable<T> GetAll(params string[] includedParams);

        /// <summary>
        /// Get all venues with included parameters
        /// </summary>
        /// <param name="includedParams">list of included parameters seperated with ';' e.g. "Resources;Resources.Reservations"</param>
        /// <returns>list of venues with included parameters</returns>
        IEnumerable<T> GetAll(string includedParams);

        /// <summary>
        /// Gets venues based on passed filters.
        /// </summary>
        /// <param name="filters">Filters a venue must pass.</param>
        /// <returns>Venues that passed all filters.</returns>
        IEnumerable<T> Get(params Func<T, bool>[] filters);

        /// <summary>
        /// Gets venues based on passed filters.
        /// </summary>
        /// <param name="includedParams"></param>
        /// <param name="filters">Filters a venue must pass.</param>
        /// <returns>Venues that passed all filters.</returns>
        IEnumerable<T> Get(string includedParams, params Func<T, bool>[] filters);

        /// <summary>
        /// Finds venue based on key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Venue with the key or null.</returns>
        Task<T> FindAsync(string key);

        /// <summary>
        /// Removes venue from database which has passed key.
        /// </summary>
        /// <param name="key">Passed key.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Update a venue in database with new values.
        /// </summary>
        /// <param name="venue">Venue which holds new values.</param>
        Task UpdateAsync(T venue);

        Task<IEnumerable<T>> Filter(T like);
    }
}