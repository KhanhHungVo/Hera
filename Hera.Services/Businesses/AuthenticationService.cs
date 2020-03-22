using Hera.Common.Data;
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class AuthenticationService : ServiceBaseTypeId<UserEntity, string>, IAuthenticationService
    {
        public AuthenticationService(IUserRepository repository) : base(repository)
        {
        }

        public async Task<UserLoginRepsonseService> GetUserLogin(string username, string hashedPassword)
        {
            var query = _repository.Query().Where(u => u.Username.Equals(username) && u.HashedPassword.Equals(hashedPassword));
            var userEntity = await query.FirstOrDefaultAsync();

            return userEntity == null ? null : new UserLoginRepsonseService
            {
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Band = userEntity.Band,
                Telephone = userEntity.PhoneNumber,
                Email = userEntity.Email,
                Onboarding = userEntity.Onboarding,
            };
        }

        public async Task<UserRegisterViewModel> Register(UserRegisterViewModel userRegister)
        {

            var userEntity = new UserEntity
            {
                Username = userRegister.Username,
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Age = userRegister.Age,
                Band = userRegister.Band,
                Email = userRegister.Email,
                PhoneNumber = userRegister.Telephone,
                HashedPassword = userRegister.Password,
            };
            userEntity.DOB = new DateTime(DateTime.UtcNow.Year - userEntity.Age, 8, 4);

            _repository.Add(userEntity);
            await _repository.SaveChangesAsync();

            return userRegister.WithoutPassword();
        }
    }
}
