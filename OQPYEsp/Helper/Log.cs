using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace OQPYEsp.Helper
{
    public class Log
    {
        static TelemetryClient telemetry = new TelemetryClient();
        public static void BasicLog(string about, string logData, SeverityLevel Sl)
        {
            if ( Sl == SeverityLevel.Information )
            {
#if DEBUG
                return;
#endif
            }

            telemetry.TrackTrace("Log",
                           SeverityLevel.Information,
                           new Dictionary<string, string> { { about, logData.ToString() } });
        }
    }
}
