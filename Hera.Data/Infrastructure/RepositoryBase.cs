using Hera.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public class RepositoryBase<T> : RepositoryBaseTypeId<T, int>, IRepositoryBase<T> where T : class, IEntityTypeId<int>
    {
        public RepositoryBase(HeraDbContext context) : base(context)
        {

        }
    }
}
