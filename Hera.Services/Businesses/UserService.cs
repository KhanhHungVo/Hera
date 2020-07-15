
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

        public async Task<UserViewModel> CreateUserAsync(UserRegisterViewModel userRegister)
        {
            var userEntity = new UserEntity
            {
                UserName = userRegister.UserName ?? userRegister.Email,
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

            return MapToViewModel(userEntity);
        }

        public async Task DeleteUser(string id)
        {
            var found = await _repository.Query().Where(u => u.Id == u.Id).FirstOrDefaultAsync();
            if (found == null) return;

            _repository.Delete(found);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserViewModel>> GetListAsync()
        {
            List<UserEntity> users = await _repository.QueryAsNoTracking().ToListAsync();
            return users.Select(MapToViewModel).ToList();
        }

        public async Task<UserViewModel> GetById(string id)
        {
            var user = await _userRepository.QueryAsNoTracking()
                                   .Where(u => u.Id == id).FirstOrDefaultAsync();
            return MapToViewModel(user);
        }

        public async Task<UserViewModel> GetByLoginAsync(string username, string hashedPassword)
        {
            var query = _userRepository.QueryAsNoTracking()
                                   .Where(u => username.Equals(u.UserName) &&
                                               hashedPassword.Equals(u.HashedPassword));
            var userEntity = await query.FirstOrDefaultAsync();

            return MapToViewModel(userEntity);
        }

        public async Task<UserViewModel> UpdateUser(UserViewModel model)
        {
            var userEntity = MapFromViewModel(model);
            await _userRepository.UpdateAsync(userEntity);
            return MapToViewModel(userEntity);
        }

        public UserViewModel MapToViewModel(UserEntity userEntity)
        {
            return userEntity == null ? null : new UserViewModel
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Band = userEntity.Band,
                PhoneNumber = userEntity.PhoneNumber,
                Email = userEntity.Email,
                Onboarding = userEntity.Onboarding,
                ProfilePicture = userEntity.ProfilePicture
            };
        }

        public UserEntity MapFromViewModel(UserViewModel model)
        {
            return new UserEntity
            {
                Id = model.Id,
                UserName = model.UserName,
                Band = model.Band,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ProfilePicture = model.ProfilePicture,
                Onboarding = model.Onboarding
            };
        }

        public async Task<UserViewModel> FindUserOrCreate(UserEntity userEntity)
        {
            var user = await GetByEmail(userEntity.Email);
            if (user == null)
            {
                _repository.Add(userEntity);
                await _repository.SaveChangesAsync();
                user = MapToViewModel(userEntity);
            } 
            return user;
        }

        public async Task<UserViewModel> FindUserOrCreateFromSocialAccount(SocialUserInfo socialUser)
        {
            var user = await GetByEmail(socialUser.Email);
            
            if (user == null)
            {
                var userEntity = new UserEntity
                {
                    UserName = socialUser.Email,
                    FirstName = socialUser.FirstName,
                    LastName = socialUser.LastName,
                    Email = socialUser.Email,
                    ProfilePicture = socialUser.ProfilePicture,
                };
                _repository.Add(userEntity);
                await _repository.SaveChangesAsync();
                user = MapToViewModel(userEntity);
            } else
            {
                user.ProfilePicture = socialUser.ProfilePicture;
                await UpdateUser(user);
            }
            return user;
        }

        public async Task<UserViewModel> GetByEmail(string email)
        {
            var query = _repository.QueryAsNoTracking()
                                   .Where(u => u.Email == email);
            var userEntity = await query.FirstOrDefaultAsync();

            return MapToViewModel(userEntity);
        }

        public async Task<string> ValidateUserDefinedRules(UserRegisterViewModel model)
        {
            var userWithSameEmail = await GetByEmail(model.Email);
            if (userWithSameEmail != null)
            {
                return "Email is already exist";
            }
            return null;
        }
    }
}
