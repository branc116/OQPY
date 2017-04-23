using OQPYModels.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories.Interfaces
{
    public interface IBaseDbRepository<T> where T : ICoreModel<T>
    {
        void OnCreate();
        IQueryable<T> Filter(T like);
    }
}
