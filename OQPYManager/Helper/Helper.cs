using Microsoft.EntityFrameworkCore;
using OQPYManager.Data;
using OQPYModels.Models.CoreModels;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Helper
{
    public static class Helper
    {
        public async static Task<Venue> GetVenueAsync(ApplicationDbContext context, string venueId)
        {
            return await context.Venues.FirstOrDefaultAsync(i => i.Id == venueId);
        }

        /// <summary>
        /// Get the venue from the context with specific Id and it will have non dbset properties and dbset properties that are included
        /// </summary>
        /// <param name="context">context from whitch one want's to get venue</param>
        /// <param name="venueId">venue id</param>
        /// <param name="includes">properties that will be included e.g. Reviews</param>
        /// <returns>venue that has properties or null</returns>
        public async static Task<Venue> GetVenueAsync(ApplicationDbContext context, string venueId, params string[] includes)
        {
            var venue = context.Venues.Where(i => i.Id == venueId);
            foreach (var include in includes)
            {
                venue.Include(include);
            }
            return await venue.FirstOrDefaultAsync();
        }
    }
}