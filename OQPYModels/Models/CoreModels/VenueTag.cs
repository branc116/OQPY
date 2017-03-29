namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// Class used to realise many-to-many relationship.
    /// Check here:
    /// https://docs.microsoft.com/en-us/ef/core/modeling/relationships
    /// </summary>
    public class BaseVenueTag
    {
        public virtual string VenueId { get; set; }
        public virtual BaseVenue Venue { get; set; }

        public virtual string TagId { get; set; }
        public virtual BaseTag Tag { get; set; }
    }
}