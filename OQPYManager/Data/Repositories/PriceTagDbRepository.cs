using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class PriceTagDbRepository : BaseDbRepository<PriceTag>
    {
        public PriceTagDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.PriceTags;
        }

        public override Task<IQueryable<PriceTag>> Filter(PriceTag like)
        {
            throw new NotImplementedException();
        }
    }
}
