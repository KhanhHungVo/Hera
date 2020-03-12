using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hera.Data.Infrastructure
{
    public class HeraDbContext : DbContext
    {
        public HeraDbContext()
        {

        }

        public HeraDbContext(DbContextOptions<HeraDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var heraModelBuilders = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            {
                return typeof(IHeraCustomModelBinder).IsAssignableFrom(type);
            });

            foreach (var builderType in heraModelBuilders)
            {
                if (builderType != null && builderType != typeof(IHeraCustomModelBinder))
                {
                    var builder = (IHeraCustomModelBinder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }
    }
}
