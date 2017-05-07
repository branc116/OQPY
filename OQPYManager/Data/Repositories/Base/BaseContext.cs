using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OQPYManager.Data.Repositories.Interfaces;
namespace OQPYManager.Data.Repositories.Base
{
    
    public abstract class BaseContext : IBaseContext
    {
        protected readonly ApplicationDbContext _context;
        public BaseContext(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
