using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Repositories
{
    public class OrderRepository : RepositoryBaseTypeId<OrderEntity,int>
    {
        public OrderRepository(HeraDbContext context): base(context)
        {

        }
    }
}
