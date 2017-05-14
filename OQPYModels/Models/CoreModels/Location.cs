using System;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// This class is used as a model for a venue to describe its location.
    /// It should be compactible with google or facebook so we can use their services.
    /// </summary>
    public class Location: ICoreModel<Location>
    {

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
            return one != null && two != null && (one.ToKilometers(two) < 10);
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
        public double  ToKilometers(Location to)
        { 
            var R = Constants.EarthRadius;
            var dLat = to.Latitude * Math.PI / 180 - this.Latitude * Math.PI / 180;
            var dLon = to.Longditude * Math.PI / 180 - this.Longditude * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(this.Latitude * Math.PI / 180) * Math.Cos(to.Latitude * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return Math.Round(d,1);
        }
    }
}