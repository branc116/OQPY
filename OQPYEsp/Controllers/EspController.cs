using Microsoft.AspNetCore.Mvc;
using OQPYEsp.DataStore;
using OQPYEsp.Models;

namespace OQPYEsp.Controllers
{
    [Route("api1/esp")]
    public class EspController: Controller
    {
        private EspDataStore _dataStore = new EspDataStore();

        [HttpGet]
        public string GetEsp([FromHeader]string espId, [FromHeader]string rawData, [FromHeader]string onOff)
        {
            int raw = 0, on = 0;
            var retStr = $"EspId = {espId} rawData = {rawData} on = {onOff}";
            bool parsed = true;
            parsed = parsed && int.TryParse(rawData, out raw);
            parsed = parsed && int.TryParse(onOff, out on);
            if ( parsed )
            {
                var newData = new ESPData(on == 1, raw, espId);
                _dataStore.addToStore(newData);
            }
            System.Console.WriteLine(retStr);
            return retStr;
        }
    }
}