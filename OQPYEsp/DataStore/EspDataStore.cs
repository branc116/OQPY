using OQPYEsp.Models;
using System;
using System.Collections.Generic;

namespace OQPYEsp.DataStore
{
    public class EspDataStore
    {
        private static List<ESPData> _espData = new List<ESPData>();

        public void addToStore(ESPData data)
        {
            _espData.Add(data);
        }

        public void addToStore(params ESPData[] data)
        {
            _espData.AddRange(data);
        }

        public IEnumerable<ESPData> GetData()
        {
            return _espData;
        }

        public void RemoveAllData(Predicate<ESPData> predicate)
        {
            _espData.RemoveAll(predicate);
        }
    }
}