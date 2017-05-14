using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace OQPYManager.Helper
{
    public class Log
    {
        private static TelemetryClient telemetry = new TelemetryClient();

        public static void BasicLog(string about, string logData, SeverityLevel Sl)
        {
            if (Sl == SeverityLevel.Information)
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