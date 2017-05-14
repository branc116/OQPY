#define ENABLE_CRAWL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using OQPYCralwer;
using OQPYManager.Data.Repositories.Base;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using static OQPYModels.Helper.Helper;
using static OQPYManager.Helper.Log;
using Location = GoogleApi.Entities.Common.Location;

namespace OQPYManager.Data.Repositories
{
    public class VenuesDbRepository : BaseDbRepository<Venue>, IVenuesDbRepository
    {
        private const string TAG = "VenueDb";

        public VenuesDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Venues;
        }


        public override async Task AddAsync(Venue venue)
        {
            try
            {
                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
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


        //I may change implementation a bit beacuse we may need to
        //load some other info(like owner and such)
        public override async Task<Venue> FindAsync(string key)
        {
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
                if (entity != null)
                {
                    if (entity.VenueTags != null)
                        _context.RemoveRange(entity.VenueTags);
                    if (entity.Tags != null)
                        _context.RemoveRange(entity.Tags);
                    _context.RemoveRange(entity.Reviews);
                    if (entity.Resources != null)
                        _context.RemoveRange(entity.Resources);
                    if (entity.PriceTags != null)
                        _context.RemoveRange(entity.PriceTags);
                    if (entity.Location != null)
                        _context.RemoveRange(entity.Location);
                    _context.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        public override async Task<IQueryable<Venue>> Filter(Venue like)
        {
            try
            {
                var venues = _context.Venues
                    .Include(i => i.Tags)
                    .Include(i => i.Location)
                    .AsQueryable();

                if (like.Location != null)
                    venues = venues
                        .Where(i => i.Location != null)
                        .Where(i => i.Location
                            .Filter(i.Location, like.Location));


                if (like.Name != null)
                    venues = venues
                        .Where(i => i.Name.StringLikenes(like.Name) > 100);
                if (like.Location != null)
                    venues = venues
                        .OrderBy(i => i.Location
                            .DistanceInDegrees(like.Location));
                else
                    venues = venues
                        .OrderByDescending(i => i.Name.StringLikenes(like.Name));
#if ENABLE_CRAWL
                try
                {
                    var crawl = new Cralw();
                    List<Venue> newVenues = new List<Venue>();
                    if (like.Name != null)
                        newVenues.AddRange(await crawl.CrawlByText(like.Name));
                    else if (like.Location != null)
                        newVenues.AddRange(await crawl.CrawlByLocation(
                                new Location(like.Location.Latitude, like.Location.Longditude)));
                    newVenues = newVenues.Where(i => { return _context.Venues.All(j => j.Id != i.Id); }).ToList();
                    await AddAsync(newVenues);
                }
                catch (Exception ex)
                {
                    BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                }
#endif
                return venues;
            }
            catch (Exception ex)
            {
                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                throw;
            }
        }
    }
}