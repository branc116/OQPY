namespace OQPYModels.Models.CoreModels
{
    public class FacebookLocation
    {
        public Geo geo { get; set; }

        public override string ToString() => geo.ToString();
    }

    public class Geo
    {
        public float elevation { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string type { get; set; }

        public override string ToString() => $"({latitude}, {longitude})";
    }
}