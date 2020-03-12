using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Data
{
    public abstract class EntityBaseTypeId<TId> : IEntityTypeId<TId>
    {
        public TId Id { get; protected set; }
    }
}
