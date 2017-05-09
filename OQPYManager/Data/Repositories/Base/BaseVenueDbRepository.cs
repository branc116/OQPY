using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseVenueDbRepository: BaseDbRepository<Venue>, IVenuesDbRepository
    {
        public BaseVenueDbRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}