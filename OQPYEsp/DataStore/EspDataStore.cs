using OQPYEsp.Models;
using System;
using System.Collections.Generic;

namespace OQPYEsp.DataStore
{
    public class EspDataStore
    {
        private static Queue<ESPData> _espData = new Queue<ESPData>();
        private static List<ESPData> _oldData = new List<ESPData>();
        //public static event EventHandler<EventArgs> NewData;

        public void AddToStore(ESPData data)
        {
            _espData.Enqueue(data);
            //NewData?.Invoke(null, EventArgs.Empty);
        }

        public void AddToStore(params ESPData[] data)
        {
            foreach ( var _ in data )
                AddToStore(_);
        }

        public ESPData Pop()
        {
            if ( _espData.Count > 0 )
            {
                var cur = _espData.Dequeue();
                _oldData.Add(cur);
                return cur;
            }
            return null;
        }
        public IEnumerable<ESPData> GetData()
        {
            return _oldData;
        }

        //public void RemoveAllData(Predicate<ESPData> predicate)
        //{
        //    _espData.RemoveAll(predicate);
        //}
    }
}