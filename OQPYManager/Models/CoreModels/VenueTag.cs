namespace OQPYManager.Models.CoreModels
{
    /// <summary>
    /// Class used to realise many-to-many relationship.
    /// Check here:
    /// https://docs.microsoft.com/en-us/ef/core/modeling/relationships
    /// </summary>
    public class VenueTag
    {
        public string VenueId { get; set; }
        public Venue Venue { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}