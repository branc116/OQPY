using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OQPYManager.Models;
using OQPYManager.Models.CoreModels;

namespace OQPYManager.Data
{
    public class VenuesDbContext : DbContext
    {
        //everything was moved into applivcationDbContext


        

        public VenuesDbContext(DbContextOptions<VenuesDbContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            

        }
    }
}
