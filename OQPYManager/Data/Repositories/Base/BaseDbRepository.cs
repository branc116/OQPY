using Microsoft.ApplicationInsights.DataContracts;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OQPYManager.Helper.Log;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseDbRepository<T>: BaseContext, IBaseDbRepository<T> where T : class, ICoreModel<T>
    {
        private const string TAG = "BaseDb";

        public BaseDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public abstract Task OnCreate();

        public abstract Task<IEnumerable<T>> Filter(T like);

        public virtual async Task AddAsync(T venue)
        {
            await _context.AddAsync(venue);
        }

        public virtual async Task AddAsync(params T[] venues)
        {
            await _context.AddRangeAsync(venues);
        }

        public virtual async Task AddAsync(IEnumerable<T> venues)
        {
            await _context.AddRangeAsync(venues);
        }

        /// <summary>
        /// can't make it generic...
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> GetAll();

        /// <summary>
        /// can't make it generic...
        /// </summary>
        /// <param name="includedParams"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetAll(params string[] includedParams);

        /// <summary>
        /// can't make it generic...
        /// </summary>
        /// <param name="includedParams"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetAll(string includedParams);

        /// <summary>
        /// can't make it generic...
        /// </summary>
        /// <param name="includedParams"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Get(params Func<T, bool>[] filters);

        /// <summary>
        /// can't make it generic...
        /// </summary>
        /// <param name="includedParams"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Get(string includedParams, params Func<T, bool>[] filters);

        public virtual async Task<T> FindAsync(string key)
        {
            T item = await _context.FindAsync<T>(key);
            if ( item == null )
            {
                BasicLog(TAG, $"Id not found = {key}", SeverityLevel.Error);
                throw new KeyNotFoundException(key);
            }
            return item;
        }

        public virtual async Task RemoveAsync(string key)
        {
            T item = await FindAsync(key);
            _context.Remove(item);
        }

        public virtual async Task UpdateAsync(T venue)
        {
            _context.Update(venue);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch ( Exception ex )
            {
                BasicLog(TAG, $"Id not found", SeverityLevel.Error);
                throw new KeyNotFoundException();
            }
        }
    }
}