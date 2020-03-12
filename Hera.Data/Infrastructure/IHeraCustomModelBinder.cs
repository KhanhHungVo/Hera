using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public interface IHeraCustomModelBinder
    {
        void Build(ModelBuilder binder);
    }
}
