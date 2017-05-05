using System;
using System.Collections.Generic;
using System.Linq;

namespace OQPYEsp.Models
{
    public class ESPData
    {
        private static List<string> _keys = new List<string>() { "OnOff", "esp-id", "raw-data" };
        public bool OnOff { get; set; }
        public int RawHall { get; set; }
        public string EspId { get; set; }
        public DateTime time { get; }

        public ESPData()
        {
            time = DateTime.Now;
        }

        public ESPData(bool onOff, int rawHall, string espId) : this()
        {
            OnOff = onOff;
            RawHall = rawHall;
            EspId = espId;
        }

        public static bool TryParse(string espRawData, out ESPData data)
        {
            try
            {
                int tempRawHall = 0;
                int tempOnOff = 0;
                data = new ESPData();
                var keyValues = espRawData.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var values = (from _ in keyValues
                              select _.Split(':')[1].Replace(" ", string.Empty)).ToList();
                bool outRet = true;
                data.EspId = values[0];
                System.Console.WriteLine("\n\nStarted parsing:\n");
                System.Console.WriteLine($"EspId = {values[0]}\n");
                outRet = outRet && int.TryParse(values[1], out tempRawHall);
                System.Console.WriteLine($"RawHall string = {values[1]} parsed = {tempRawHall} outRet = {outRet}");
                outRet = outRet && int.TryParse(values[2], out tempOnOff);
                System.Console.WriteLine($"OnOff string = {values[2]} parsed = {tempOnOff} outRet = {outRet}");
                if ( outRet )
                {
                    data.OnOff = tempOnOff == 1;
                    data.RawHall = tempRawHall;
                    return true;
                }
                else
                {
                    data = null;
                    return false;
                }
            }
            catch ( Exception ex )
            {
                System.Console.WriteLine(ex);
                data = null;
                return false;
            }
        }
    }
}