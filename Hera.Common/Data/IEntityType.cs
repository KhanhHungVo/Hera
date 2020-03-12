using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Data
{
    public interface IEntityTypeId<TId>
    {
        TId Id { get; }
    }
}
