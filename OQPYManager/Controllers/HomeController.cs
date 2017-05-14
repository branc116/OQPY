using Microsoft.AspNetCore.Mvc;
using OQPYManager.Data.Repositories;
using OQPYManager.Data.Repositories.Interfaces;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Controllers
{
    public class HomeController : Controller
    {
        private IVenuesDbRepository _venuesDbRepository;
        public HomeController(IVenuesDbRepository venuesDbRepository)
        {
            _venuesDbRepository = venuesDbRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Venue()
        {
            return View();
        }


        public IActionResult VenueCreation(Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return View("Venue", venue);
            }

            _venuesDbRepository.AddAsync(venue);

            return View("Index");
        }
    }
}