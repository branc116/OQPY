using System.Linq;

namespace OQPYManager.Data.Repositories
{
    public abstract class BaseDbRepository
    {
        protected readonly ApplicationDbContext _context;

        public BaseDbRepository(ApplicationDbContext context)
        {
            _context = context;

            if (!_context.Venues.Any())
            {
                OnCreate();
            }
        }

        public abstract void OnCreate();
    }
}