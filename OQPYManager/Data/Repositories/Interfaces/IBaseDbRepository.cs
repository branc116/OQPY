using OQPYModels.Models.CoreModels;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IBaseDbRepository<T> where T : ICoreModel<T>
    {
        void OnCreate();

        Task<IQueryable<T>> Filter(T like);
    }
}