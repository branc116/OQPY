using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OQPYManager.Data.Repositories
{
    using Base;
    using OQPYModels.Models.CoreModels;

    public class ResourceDbRepository: BaseResourceDbRepository
    {
        public ResourceDbRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override Task<IQueryable<Resource>> Filter(Resource like) => throw new NotImplementedException();
        public override IEnumerable<Resource> Get(params Func<Resource, bool>[] filters) => throw new NotImplementedException();
        public override IEnumerable<Resource> Get(string includedParams, params Func<Resource, bool>[] filters) => throw new NotImplementedException();
        public override IEnumerable<Resource> GetAll() => throw new NotImplementedException();
        public override IEnumerable<Resource> GetAll(params string[] includedParams) => throw new NotImplementedException();
        public override IEnumerable<Resource> GetAll(string includedParams) => throw new NotImplementedException();
        public override Task OnCreate() => throw new NotImplementedException();
    }
}
