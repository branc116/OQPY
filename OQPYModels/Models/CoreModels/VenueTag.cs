namespace OQPYModels.Models.CoreModels
{
    /// <summary>
    /// Class used to realise many-to-many relationship.
    /// Check here:
    /// https://docs.microsoft.com/en-us/ef/core/modeling/relationships
    /// </summary>
    public class VenueTag
    {
        public virtual string VenueId { get; set; }
        public virtual Venue Venue { get; set; }

        public virtual string TagId { get; set; }
        public virtual Tag Tag { get; set; }

        public VenueTag()
        {
        }

        public VenueTag(Venue venue, Tag tag)
        {
            this.Tag = tag;
            this.Venue = venue;
            this.TagId = tag.Id;
            this.VenueId = venue.Id;
        }
    }
}