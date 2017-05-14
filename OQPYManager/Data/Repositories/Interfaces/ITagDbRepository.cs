using OQPYModels.Models.CoreModels;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface ITagDbRepository : IBaseDbRepository<Tag>
    {
        /// <summary>
        /// This method should add new tag to database and relate that tag to its Venue.
        /// </summary>
        /// <param name="tag">Tag to be made</param>
        /// <param name="venueId">Id of the venue with which a tag will be joined</param>
        Task AddAsync(Tag tag, string venueId);
    }
}