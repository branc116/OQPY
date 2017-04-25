using System;

namespace OQPYModels.Models.CoreModels
{
    public class Location: ICoreModel<Location>
    {
        public virtual string Id { get; set; }
        public virtual double Longditude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual string Adress { get; set; }

        public bool Filter(Location one, Location two)
        {
            var dx = one?.Latitude - two?.Latitude ?? 0;
            var dy = one?.Longditude - two?.Longditude ?? 0;
            return Math.Sqrt((dx * dx) + (dy * dy)) < 0.001 ||
                (one?.Adress?.Contains(two?.Adress) ?? false) ||
                (two?.Adress?.Contains(one?.Adress) ?? false);
        }
    }
}