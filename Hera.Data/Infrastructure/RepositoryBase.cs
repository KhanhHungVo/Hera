using Hera.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public class RepositoryBase<T> : RepositoryBaseTypeId<T, long>, IRepositoryBaseTypeId<T> where T : class, IEntityTypeId<long>
    {
        public RepositoryBase(HeraDbContext context) : base(context)
        {

        }
    }
}
