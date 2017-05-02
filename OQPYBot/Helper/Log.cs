using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;

namespace OQPYBot.Helper
{
    public static class Log
    {
        public static void BasicLog(string about, object logData, SeverityLevel Sl)
        {
            if ( Sl == SeverityLevel.Information )
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

        internal static void BasicLog(IMessageActivity a)
        {
#if DEBUG
            BasicLog("Attachments",
                    (a.Attachments != null && a.Attachments.Any()) ?
                        (from _ in a.Attachments
                         select _.Content).Aggregate((i, j) => $"{i}\n{j}") :
                         "No Attachments",
                    SeverityLevel.Information);
#endif
        }
    }
}