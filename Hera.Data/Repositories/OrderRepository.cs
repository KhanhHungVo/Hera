﻿using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Data.Repositories
{
    public class OrderRepository : RepositoryBaseTypeId<OrderEntity,int>, IOrderRepository
    {
        public OrderRepository(HeraDbContext context): base(context)
        {

        }

      
    }
}
