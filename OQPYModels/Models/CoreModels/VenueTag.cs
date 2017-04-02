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
        public BaseVenueTag()
        {

        }
        public BaseVenueTag(BaseTag tag, BaseVenue venue)
        {
            this.Tag = tag;
            this.Venue = venue;
            this.TagId = tag.Id;
            this.VenueId = venue.Id;
        }
    }
}