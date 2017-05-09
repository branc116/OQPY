using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IBaseDbRepository<T> where T : class
    {
        
        Task AddAsync(T item);

        Task AddAsync(params T[] items);

        Task AddAsync(IEnumerable<T> items);

        IEnumerable<T> GetAll(DbSet<T> dbSet);

        /// <summary>
        /// Get all objects with included parameters
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="includedParams">list of included parameters e.g. "Resources"</param>
        /// <returns>list of objects of type T with included parameters</returns>
        IEnumerable<T> GetAll(DbSet<T> dbSet = null, params string[] includedParams);

        /// <summary>
        /// Get all objects with included parameters
        /// </summary>
        /// <param name="includedParams">list of included parameters seperated with ';' e.g. "Resources;Resources.Reservations"</param>
        /// <param name="dbSet"></param>
        /// <returns>list of objects of type T with included parameters</returns>
        IEnumerable<T> GetAll(string includedParams, DbSet<T> dbSet = null);

        /// <summary>
        /// Gets objects based on passed filters.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="filters">Filters an object must pass.</param>
        /// <returns>Objects that passed all filters.</returns>
        IEnumerable<T> Get(DbSet<T> dbSet = null, params Func<T, bool>[] filters);

        /// <summary>
        /// Gets objects based on passed filters.
        /// </summary>
        /// <param name="includedParams"></param>
        /// <param name="dbSet"></param>
        /// <param name="filters">Filters a object must pass.</param>
        /// <returns>objects that passed all filters.</returns>
        IEnumerable<T> Get(string includedParams, DbSet<T> dbSet = null, params Func<T, bool>[] filters);

        /// <summary>
        /// Finds object based on key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>object with the key or null.</returns>
        Task<T> FindAsync(string key);

        /// <summary>
        /// Removes object from database which has passed key.
        /// </summary>
        /// <param name="key">Passed key.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Update a object in database with new values.
        /// </summary>
        /// <param name="item">item which holds new values.</param>
        Task UpdateAsync(T item);

        Task<IQueryable<T>> Filter(T like);
    }
}