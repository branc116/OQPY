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
        Task<IEnumerable<Resource>> GetAllInVenue(string venueId);
    }
}
