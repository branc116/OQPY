using Microsoft.EntityFrameworkCore;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Extensions;
using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static OQPYModels.Helper.Helper;
namespace OQPYManager.Data.Repositories
{
    public class VenuesDbRepository: BaseVenueDbRepository
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
            foreach ( var param in includedParams )
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
            foreach ( var filter in filters )
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

        public async override Task<IQueryable<Venue>> Filter(Venue like)
        {

            var venues = _context.Venues
                .Include(i => i.Tags)
                .AsQueryable();

            if ( like.Location != null )
                venues = venues
                    .Where(i => i.Location != null)
                    .Where(i => i.Location
                        .Filter(i.Location, like.Location))
                    .Include(i => i.Location);

            if ( like.Name != null )
                venues = venues
                    .Where(i => i.Name != null)
                    .Where(i => i.Name.StringLikenes(like.Name) > 100);
            if(like.Location != null)
                venues = venues
                    .OrderBy(i => i.Location
                        .DistanceInDegrees(like.Location));
            else 
                venues = venues
                    .OrderByDescending(i => i.Name.StringLikenes(like.Name));
            if ( venues.Count() < 10 )
            {
                try
                {
                    var crawl = new OQPYCralwer.Cralw();
                    IEnumerable<Venue> newVenues;
                    if ( like.Location == null && like.Name != null )
                        newVenues = await crawl.CrawlByText(like.Name);
                    else if ( like.Name == null && like.Location != null )
                        newVenues = await crawl.CrawlByLocation(new GoogleApi.Entities.Common.Location(like.Location.Latitude, like.Location.Longditude));
                    else if ( like.Name != null && like.Location != null )
                        newVenues = await crawl.CrawlSimlar(like);
                    else
                        return null;
                    newVenues = newVenues.Where((i) =>
                    {
                        return _context.Venues.All(j => j.Id != i.Id);
                    });
                    await AddVenuesAsync(newVenues);
                }catch(Exception ex )
                {
                }
            }
            return venues;
        }
    }
}