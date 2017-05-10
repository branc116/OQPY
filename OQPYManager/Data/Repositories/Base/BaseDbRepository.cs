using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using static OQPYManager.Helper.Log;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseDbRepository<T> : IBaseDbRepository<T> where T : class
    {
        private const string TAG = "BaseDb";
        protected readonly ApplicationDbContext _context;
        protected DbSet<T> _defaultDbSet;

        public BaseDbRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     This filter is used when searching trough database to search for similar items to 
        ///     like item(the parameter).
        /// </summary>
        /// <param name="like">Items to be compared to.</param>
        /// <returns></returns>
        public abstract Task<IQueryable<T>> Filter(T like);

        /// <summary>
        /// Adds an item to Db
        /// </summary>
        /// <param name="item">Item to be added to Db</param>
        /// <returns></returns>
        public virtual async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
        }

        /// <summary>
        /// Adds range of items
        /// </summary>
        /// <param name="items">Range of items</param>
        /// <returns></returns>
        public virtual async Task AddAsync(params T[] items)
        {
            await AddAsync(items.ToList());
        }

        /// <summary>
        /// Adds range of items
        /// </summary>
        /// <param name="items">Range of items</param>
        /// <returns></returns>
        public virtual async Task AddAsync(IEnumerable<T> items)
        {
            await _context.AddRangeAsync(items);
        }

        /// <summary>
        ///     Gets all items from a dbSet.
        /// </summary>
        /// <returns>Queryable items retreieved from dbSet.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _defaultDbSet.AsQueryable();
        }

        /// <summary>
        ///     Gets all items from dbSet based on parameters.
        ///     DbSet parameter is optional because we want as less retyping as possible of same algorithm.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="includedParams"></param>
        /// <returns>Items from dbSet that pass parameter.</returns>
        public virtual IEnumerable<T> GetAll(params string[] includedParams)
        {
            var items = GetAll().AsQueryable();
            foreach (var param in includedParams)
                items.Include(param);
            return items;
        }

        /// <summary>
        ///     Gets all items from dbSet based on parameters.
        /// </summary>
        /// <param name="includedParams">Singular string that contains semicolons.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll(string includedParams)
        {
            return GetAll(includedParams.Split(new char[1] {';'}, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        ///     Get items based on certain filters.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Get(params Func<T, bool>[] filters)
        {
            var items = _defaultDbSet.AsQueryable();
            foreach ( var filter in filters )
            {
                items = items.Where(filter).AsQueryable();
            }
            return items;
        }

        /// <summary>
        ///     Get items based on certain filters.
        /// </summary>
        /// <param name="includedParams"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Get(string includedParams, params Func<T, bool>[] filters)
        {
            var items = GetAll(includedParams).AsQueryable();
            foreach (var filter in filters)
            {
                items = items.Where(filter).AsQueryable();
            }

            return items;
        }

        /// <summary>
        ///     Find an item in Db based on its unique key
        /// </summary>
        /// <param name="key">Key to search by.</param>
        /// <returns>Item that was searched or null if item with the passed key does not exist.</returns>
        public virtual async Task<T> FindAsync(string key)
        {
            var item = await _context.FindAsync<T>(key);
            if (item == null)
            {
                BasicLog(TAG, $"Id not found = {key}", SeverityLevel.Error);
                throw new KeyNotFoundException(key);
            }
            return item;
        }

        /// <summary>
        ///     Removes an item from database with given unique key.
        /// </summary>
        /// <param name="key">Given unique key.</param>
        public virtual async Task RemoveAsync(string key)
        {
            var item = await FindAsync(key);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///     Updates item to its new values
        /// </summary>
        /// <param name="item">Item to be updated with new values</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(T item)
        {
            _context.Update(item);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BasicLog(TAG, $"Id not found", SeverityLevel.Error);
                throw new KeyNotFoundException();
            }
        }
    }
}