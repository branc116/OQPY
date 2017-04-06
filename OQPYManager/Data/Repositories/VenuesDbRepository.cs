using System;
using System.Collections.Generic;
using System.Linq;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data
{
    public class VenuesDbRepository : IVenuesDbRepository
    {
        private readonly ApplicationDbContext _context;

        public VenuesDbRepository(ApplicationDbContext context)
        {
            _context = context;

            if (!_context.Venues.Any())
            {
                AddVenue(new Venue("Josip", "WWWW", "None", "This ONe"));
            }

        }

        public void AddVenue(Venue venue)
        {
            _context.Venues.Add(venue);
            _context.SaveChanges();
        }

        public IEnumerable<Venue> GetAllVenues()
        {
            return _context.Venues.AsQueryable();
        }


        public IEnumerable<Venue> GetVenues(params Func<Venue, bool>[] filters)
        {
            var collection = GetAllVenues();

            foreach (var filter in filters)
            {
                collection = collection.Where(filter);
            }

            return collection;
        }

        //I may change implementation a bit beacuse we may need to
        //load some other info(like owner and such)
        public Venue FindVenue(string key)
        {
            return _context.Venues.Find(key);
        }

        public void Remove(string key)
        {
            var entity = FindVenue(key);
            _context.Venues.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Venue venue)
        {
            _context.Venues.Update(venue);
            _context.SaveChanges();
        }

        
    }
}
