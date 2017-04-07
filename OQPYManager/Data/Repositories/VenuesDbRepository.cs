using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Interface;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories
{
    public class VenuesDbRepository : BaseDbRepository, IVenuesDbRepository
    {
        public VenuesDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override void OnCreate()
        {
            Task.WaitAll(AddVenueAsync(new Venue("Josip", "WWWW", "None", "This ONe")));
        }

        public async Task AddVenueAsync(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        public async Task AddVenuesAsync(params Venue[] venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public async Task AddVenuesAsync(IEnumerable<Venue> venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Venue> GetAllVenues()
        {
            return _context.Venues.AsQueryable();
        }

        public IEnumerable<Venue> GetAllVenues(params string[] includedParams)
        {
            var venues = _context.Venues.AsQueryable();
            foreach (var param in includedParams)
                venues.Include(param);
            return venues;
        }

        public IEnumerable<Venue> GetAllVenues(string includedParams)
        {
            return GetAllVenues(includedParams.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters)
        {
            return GetVenues(string.Empty, filters);
        }

        public IEnumerable<Venue> GetVenues(string includedParams, params Func<Venue, bool>[] filters)
        {
            var venues = GetAllVenues(includedParams);
            foreach (var filter in filters)
            {
                venues = venues.Where(filter);
            }

            return venues;
        }

        //I may change implementation a bit beacuse we may need to
        //load some other info(like owner and such)
        public async Task<Venue> FindVenueAsync(string key)
        {
            return await _context.Venues.FindAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            var entity = await FindVenueAsync(key);
            _context.Venues.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }
    }
}