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
using static OQPYManager.Helper.Log;
using Microsoft.ApplicationInsights.DataContracts;

namespace OQPYManager.Data.Repositories
{
    public class VenuesDbRepository: BaseVenueDbRepository
    {
        private const string TAG = "VenueDb";
        public VenuesDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async override Task OnCreate()
        {
            await Task.WhenAll(AddAsync(new Venue("Josip", "WWWW", "None", "This ONe")));
        }

        public override async Task AddAsync(Venue venue)
        {
            try
            {
                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
            }catch(Exception ex )
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
            }
        }

        public override async Task AddAsync(params Venue[] venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public override async Task AddAsync(IEnumerable<Venue> venues)
        {
            _context.Venues.AddRange(venues);
            await _context.SaveChangesAsync();
        }

        public override IEnumerable<Venue> GetAll()
        {
                return _context.Venues.AsQueryable();
        }

        public override IEnumerable<Venue> GetAll(params string[] includedParams)
        {
            var venues = _context.Venues.AsQueryable();
            foreach ( var param in includedParams )
                venues.Include(param);
            return venues;
        }

        public override IEnumerable<Venue> GetAll(string includedParams)
        {
            return GetAll(includedParams.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public override IEnumerable<Venue> Get(params Func<Venue, bool>[] filters)
        {
            return Get(string.Empty, filters);
        }

        public override IEnumerable<Venue> Get(string includedParams, params Func<Venue, bool>[] filters)
        {
            var venues = GetAll(includedParams);
            foreach ( var filter in filters )
            {
                venues = venues.Where(filter);
            }

            return venues;
        }

        //I may change implementation a bit beacuse we may need to
        //load some other info(like owner and such)
        public override async Task<Venue> FindAsync(string key) {
            var venue = await _context.Venues
                .Include(i => i.VenueTags)
                    .Include(i => i.Tags)
                    .Include(i => i.Reviews)
                    .Include(i => i.Resources)
                    .Include(i => i.PriceTags)
                    .Include(i => i.Location)
                    .FirstOrDefaultAsync(i => i.Id == key);
            return venue;
        }

        public override async Task RemoveAsync(string key)
        {
            try
            {
                var entity = await FindAsync(key);
                if ( entity != null )
                {
                    if ( entity.VenueTags != null )
                        _context.RemoveRange(entity.VenueTags);
                    if ( entity.Tags != null )
                        _context.RemoveRange(entity.Tags);
                    _context.RemoveRange(entity.Reviews);
                    if ( entity.Resources != null )
                        _context.RemoveRange(entity.Resources);
                    if ( entity.PriceTags != null )
                        _context.RemoveRange(entity.PriceTags);
                    if ( entity.Location != null )
                        _context.RemoveRange(entity.Location);
                    _context.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch ( Exception ex )
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                throw;
            }
        }

        public override async Task UpdateAsync(Venue venue)
        {
            try
            {
                _context.Venues.Update(venue);
                await _context.SaveChangesAsync();
            }catch(Exception ex )
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
            }
        }

        public async override Task<IQueryable<Venue>> Filter(Venue like)
        {
            try
            {
                var venues = _context.Venues
                    .Include(i => i.Tags)
                    .Include(i => i.Location)
                    .AsQueryable();

                if ( like.Location != null )
                    venues = venues
                        .Where(i => i.Location != null)
                        .Where(i => i.Location
                            .Filter(i.Location, like.Location));


                if ( like.Name != null )
                    venues = venues
                        .Where(i => i.Name.StringLikenes(like.Name) > 100);
                if ( like.Location != null )
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
                            newVenues = await crawl.CrawlByLocation(new GoogleApi.Entities.Common.Location(like.Location.Latitude, like.Location.Longditude));
                        else
                            return null;
                        newVenues = newVenues.Where((i) =>
                        {
                            return _context.Venues.All(j => j.Id != i.Id);
                        });
                        await AddAsync(newVenues);
                    }
                    catch ( Exception ex )
                    {
                        BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                    }
                }
                return venues;
            }catch(Exception ex )
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                throw;
            }
            
        }
    }
}