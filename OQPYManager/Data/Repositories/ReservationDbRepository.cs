using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Base;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public class ReservationDbRepository : BaseDbRepository<Reservation>
    {
        public ReservationDbRepository(ApplicationDbContext context) : base(context)
        {
            _defaultDbSet = _context.Reservations;
        }

        public override Task<IQueryable<Reservation>> Filter(Reservation like)
        {
            throw new NotImplementedException();
        }
    }
}
