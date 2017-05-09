using Microsoft.AspNetCore.Mvc;
using OQPYEsp.DataStore;

namespace OQPYEsp.Controllers
{
    [Route("api1/hal")]
    public class HalController: Controller
    {
        private EspDataStore _dataStore = new EspDataStore();

        [HttpGet]
        public string Hal()
        {
            string ret = string.Empty;
            foreach ( var _ in _dataStore.GetData() )
            {
                ret += $"{_.EspId} {_.OnOff} {_.RawHall} {_.time}\r\n";
            }
            return ret;
        }
    }
}