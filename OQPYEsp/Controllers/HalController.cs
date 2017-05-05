using Microsoft.AspNetCore.Mvc;
using OQPYEsp.DataStore;
using System.Linq;

namespace OQPYEsp.Controllers
{
    [Route("api1/hal")]
    public class HalController: Controller
    {
        private EspDataStore _dataStore = new EspDataStore();

        [HttpGet]
        public IActionResult Hal()
        {
            ViewData["Esp"] = _dataStore.GetData().Reverse().Take(20);
            return View();
        }
    }
}