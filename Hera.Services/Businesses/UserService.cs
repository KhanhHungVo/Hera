
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class UserService : ServiceBaseTypeId<UserEntity, string>, IUserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, ILogger logger) : base(userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<UserResponseViewModel> CreateUserAsync(UserRegisterViewModel userRegister)
        {
            var userEntity = new UserEntity
            {
                Username = userRegister.Email,
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                //Age = userRegister.Age,
                //Band = userRegister.Band,
                Email = userRegister.Email,
                PhoneNumber = userRegister.Telephone,
                HashedPassword = userRegister.Password,
            };
            userEntity.DOB = new DateTime(DateTime.UtcNow.Year - userEntity.Age, 8, 4);

            _userRepository.Add(userEntity);
            await _userRepository.SaveChangesAsync();

            return MapUserResponseFromEntity(userEntity);
        }

        public async Task DeleteUser(string id)
        {
            var found = await _repository.Query().Where(u => u.Id == u.Id).FirstOrDefaultAsync();
            if (found == null) return;

            _repository.Delete(found);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserResponseViewModel>> GetListAsync()
        {
            List<UserEntity> users =  await _repository.QueryAsNoTracking().ToListAsync();
            return users.Select(MapUserResponseFromEntity).ToList();
        }

        public async Task<UserResponseViewModel> GetById(string id)
        {
            var user = await _userRepository.QueryAsNoTracking()
                                   .Where(u => u.Id == id).FirstOrDefaultAsync();
            return MapUserResponseFromEntity(user);
        }

        public async Task<UserResponseViewModel> GetByLoginAsync(string username, string hashedPassword)
        {
            var query = _userRepository.QueryAsNoTracking()
                                   .Where(u => username.Equals(u.Username) &&
                                               hashedPassword.Equals(u.HashedPassword));
            var userEntity = await query.FirstOrDefaultAsync();

            return MapUserResponseFromEntity(userEntity);
        }

        public Task UpdateUser(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public UserResponseViewModel MapUserResponseFromEntity(UserEntity userEntity)
        {
            return userEntity == null ? null : new UserResponseViewModel
            {
                UserId = userEntity.Id,
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Band = userEntity.Band,
                Telephone = userEntity.PhoneNumber,
                Email = userEntity.Email,
                Onboarding = userEntity.Onboarding,
            };
        }

        public async Task<UserResponseViewModel> FindUserOrCreate(UserEntity userEntity)
        {
            var user = await GetByEmail(userEntity.Email);
            if (user == null)
            {
                _repository.Add(userEntity);
                await _repository.SaveChangesAsync();
                user = MapUserResponseFromEntity(userEntity);
            }
            return user;
        }

        public async Task<UserResponseViewModel> GetByEmail(string email)
        {
            var query = _repository.QueryAsNoTracking()
                                   .Where(u => u.Email == email);
            var userEntity = await query.FirstOrDefaultAsync();

            return MapUserResponseFromEntity(userEntity);
        }
    }
}
