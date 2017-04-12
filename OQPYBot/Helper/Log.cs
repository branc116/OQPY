using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace OQPYBot.Helper
{
    public static class Log
    {
        public static void BasicLog(string about, object logData, SeverityLevel Sl)
        {
            if (Sl == SeverityLevel.Information)
            {
#if DEBUG
                return;
#endif
            }
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackTrace("Log",
                           SeverityLevel.Information,
                           new Dictionary<string, string> { { about, logData.ToString() } });
            System.Diagnostics.Debug.WriteLine($"{about}({Sl}): {logData}");
        }
    }
}