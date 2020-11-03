using Hera.Common.Core;
using Hera.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services
{
    public interface IServiceBase<T> : IServiceBaseTypeId<T, long> where T : class, IEntityTypeId<long>
    {
    }
}
