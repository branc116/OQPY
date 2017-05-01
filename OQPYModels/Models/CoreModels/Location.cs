using System;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYModels.Models.CoreModels
{
    public class Location: ICoreModel<Location>
    {
        private const string _apiKey = "AIzaSyDILEdR5gAKYwZKyocx1nsKOhQev5QQ68Q";

        public virtual string Id { get; set; }
        public virtual double Longditude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual string Adress { get; set; }
        public Location(double longditude, double latitude)
        {
            this.Longditude = longditude;
            this.Latitude = latitude;
        }
        public Location()
        {

        }
        public bool Filter(Location one, Location two)
        {
            return one != null && two != null && (one.DistanceInDegrees(two) < 0.1);
            
        }
        public async Task<bool> FilterAsync(Location one, string address)
        {
            var req = new GoogleApi.Entities.Maps.Geocode.Request.GeocodingRequest()
            {
                Address = address,
                Key = _apiKey,
            };
            var res = GoogleApi.GoogleMaps.Geocode.Query(req);
            return (from _ in res.Results
                   where _.Geometry != null
                   where _.Geometry.Location != null
                   select _.Geometry.Location)
                   .Any(i => one.Filter(one,new Location() { Longditude = i.Longitude, Latitude = i.Latitude }));
        }
        public double DistanceInDegrees(Location to)
        {
            var dx = this?.Latitude - to?.Latitude ?? 0;
            var dy = this?.Longditude - to?.Longditude ?? 0;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }
        public double DistanceInDegrees(double longditude, double latitude)
        {
            return DistanceInDegrees(new Location(longditude, latitude));
        }
        public override string ToString() => $"{Adress}: ({this.Latitude}, {this.Longditude})";
    }
}