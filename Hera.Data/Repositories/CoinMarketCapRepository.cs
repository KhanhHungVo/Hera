using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Repositories
{
    public class CoinMarketCapRepository : RepositoryBaseTypeId<CoinBasicInfoEntity, int>, ICoinMarketCapRepository
    {
        public CoinMarketCapRepository(HeraDbContext dbContext) : base(dbContext)
        {
        }
    }
}
