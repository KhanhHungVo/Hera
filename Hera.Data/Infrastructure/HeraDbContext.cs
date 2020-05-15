using Hera.Common.Core;
using Hera.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
            InitializePostgresUuidV4();
        }
        public DbSet<UserEntity> entities { get; set; }
        public DbSet<TopicEntity> topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var heraModelBuilders = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            {
                return typeof(IHeraCustomModelBinder).IsAssignableFrom(type);
            });

            foreach (var builderType in heraModelBuilders)
            {
                if (builderType != null && builderType != typeof(IHeraCustomModelBinder) && builderType.IsAbstract == false)
                {
                    var builder = (IHeraCustomModelBinder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }

        private void InitializePostgresUuidV4()
        {
            using (var npgsqlConnection = new NpgsqlConnection(Database.GetDbConnection().ConnectionString))
            {
                npgsqlConnection.Open();

                using (var npgsqlCommand = npgsqlConnection.CreateCommand())
                {
                    npgsqlCommand.CommandText = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";";
                    npgsqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
