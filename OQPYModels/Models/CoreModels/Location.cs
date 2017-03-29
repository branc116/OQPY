namespace OQPYModels.Models.CoreModels
{
    public class BaseLocation
    {
        public virtual string Id { get; set; }
        public virtual double Longditude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual string Adress { get; set; }
    }
}