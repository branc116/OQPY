using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Extensions;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories
{
    public class VenuesDbRepository : BaseVenueDbRepository
    {
        public VenuesDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override void OnCreate()
        {
            Task.WaitAll(AddVenueAsync(new Venue("Josip", "WWWW", "None", "This ONe")));
        }

        public override async Task AddVenueAsync(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        public override async Task AddVenuesAsync(params Venue[] venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public override async Task AddVenuesAsync(IEnumerable<Venue> venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public override IEnumerable<Venue> GetAllVenues()
        {
            return _context.Venues.AsQueryable();
        }

        public override IEnumerable<Venue> GetAllVenues(params string[] includedParams)
        {
            var venues = _context.Venues.AsQueryable();
            foreach (var param in includedParams)
                venues.Include(param);
            return venues;
        }

        public override IEnumerable<Venue> GetAllVenues(string includedParams)
        {
            return GetAllVenues(includedParams.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public override IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters)
        {
            return GetVenues(string.Empty, filters);
        }

        public override IEnumerable<Venue> GetVenues(string includedParams, params Func<Venue, bool>[] filters)
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
        public override async Task<Venue> FindVenueAsync(string key) => await _context.Venues.FindAsync(key);

        public override async Task RemoveAsync(string key)
        {
            var entity = await FindVenueAsync(key);
            _context.Venues.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }
        public override IQueryable<Venue> Filter(Venue like) => _context.Venues.AsQueryable().Filter(like);
    }
}