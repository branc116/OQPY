using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IResourceDbRepository : IBaseDbRepository<Resource>
    {
        Task ChangeState(string id, bool newState, string secretCode);

        /// <summary>
        /// Gets all resources that are inside a single venue.
        /// </summary>
        /// <param name="venueId">Id of a venue where we extract all resources from.</param>
        /// <returns>All resources in venue with the venueId</returns>
        Task<IEnumerable<Resource>> GetAllInVenue(string venueId);
    }
}
