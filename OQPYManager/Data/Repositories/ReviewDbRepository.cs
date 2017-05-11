using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class ReviewDbRepository : BaseDbRepository<Review>
    {
        public ReviewDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Reviews;
        }


        public override Task<IQueryable<Review>> Filter(Review like)
        {
            throw new NotImplementedException();
        }
    }
}
