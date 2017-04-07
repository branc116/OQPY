using System;
using System.Collections.Generic;
using Xunit;
using OQPYManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace OQPYManagerTests
{
    
    public class ControllersTesting
    {
        //private ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>(new Dictionary<Type, IDbContextOptions>()));
        [Fact]
        public async void TestVenueController()
        {
            //var venueController = new OQPYManager.Controllers.VenuesController(_context);
            //var allVenues = venueController.GetAllVenues();
            //Assert.NotEmpty(allVenues);
            //var deleteVenue = await venueController.DeleteVenue(Guid.NewGuid().ToString());
            //deleteVenue.ToString();
            //Assert.NotNull(deleteVenue);
        }
    }
}
