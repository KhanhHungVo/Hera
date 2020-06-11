using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hera.Data.Repositories
{
    public class UserRepository : RepositoryBaseTypeId<UserEntity, string>, IUserRepository
    {
        public UserRepository(HeraDbContext context) : base(context)
        {

        }

        public async Task<UserEntity> UpdateAsync(UserEntity entity)
        {
            //DbSet.Update(entity);
            
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.Entry(entity).Property(x => x.Id).IsModified = false;
            Context.Entry(entity).Property(x => x.HashedPassword).IsModified = false;
            Context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
            Context.Entry(entity).Property(x => x.Age).IsModified = false;

            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
