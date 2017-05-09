using Microsoft.ApplicationInsights.DataContracts;
using OQPYClient.APIv03;
using OQPYEsp.DataStore;
using OQPYEsp.Models;
using System;
using System.Threading.Tasks;
using static OQPYEsp.Helper.Log;

namespace OQPYEsp.Services
{
    public static class HandleQ
    {
        private static EspDataStore _context = new EspDataStore();
        private static MyAPI _api = new MyAPI(new Uri(Environment.GetEnvironmentVariable("OQPYManagerUri") ?? "https://localhost/"));
        private const string TAG = "HandleQ";

        public static async Task PushQ()
        {
            ESPData curData;
            while ( true )
            {
                try
                {
                    if ( (curData = _context.Pop()) != null )
                    {
                        int timeout = 5;
                        while ( --timeout >= 0 )
                        {
                            try
                            {
                                var code = await _api.ApiResourcesIOTPostWithHttpMessagesAsync(curData.EspId, curData.OnOff ? "1" : "0");
                                if ( code.Response.IsSuccessStatusCode )
                                    timeout = 0;
                            }
                            catch ( Exception ex )
                            {
                                BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
                catch ( Exception ex )
                {
                    BasicLog(TAG, ex.ToString(), SeverityLevel.Error);
                }
            }
        }
    }
}