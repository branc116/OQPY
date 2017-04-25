using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseVenueDbRepository: BaseDbRepository<Venue>, IVenuesDbRepository
    {
        public BaseVenueDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public abstract Task AddVenueAsync(Venue venue);

        public abstract Task AddVenuesAsync(params Venue[] venues);

        public abstract Task AddVenuesAsync(IEnumerable<Venue> venues);

        public abstract Task<Venue> FindVenueAsync(string key);

        public abstract IEnumerable<Venue> GetAllVenues();

        public abstract IEnumerable<Venue> GetAllVenues(params string[] includedParams);

        public abstract IEnumerable<Venue> GetAllVenues(string includedParams);

        public abstract IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters);

        public abstract IEnumerable<Venue> GetVenues(string includedParams, params Func<Venue, bool>[] filters);

        public abstract Task RemoveAsync(string key);

        public abstract Task UpdateAsync(Venue venue);
    }
}