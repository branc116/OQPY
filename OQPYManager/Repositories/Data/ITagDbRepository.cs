using System.Linq;
using System.Threading.Tasks;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data.Repositories
{
    public interface ITagDbRepository
    {
        Task AddAsync(Tag tag, string venueId);
        Task<IQueryable<Tag>> Filter(Tag like);
    }
}