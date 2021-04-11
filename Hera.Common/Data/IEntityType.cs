using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Data
{
    public interface IEntityTypeId<TId>
    {
        public TId Id { get; }
    }
}
