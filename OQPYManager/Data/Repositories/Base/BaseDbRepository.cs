using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;
using System.Linq;

namespace OQPYManager.Data.Repositories.Base
{
    public abstract class BaseDbRepository<T> : IBaseDbRepository<T> where T : ICoreModel<T> 
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
        public abstract IQueryable<T> Filter(T like);
    }
}