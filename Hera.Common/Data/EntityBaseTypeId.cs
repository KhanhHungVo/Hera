using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Data
{
    public abstract class EntityBaseTypeId<TId> : IEntityTypeId<TId>
    {
        public TId Id { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime? UpdatedDate { get; protected set; }

    }
}
