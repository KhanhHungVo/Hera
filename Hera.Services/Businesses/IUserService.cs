using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface IUserService : IServiceBaseTypeId<UserEntity, string>
    {
        Task<UserResponseViewModel> CreateUserAsync(UserRegisterViewModel userRegister);
        Task<IEnumerable<UserResponseViewModel>> GetListAsync();
        Task<UserResponseViewModel> GetById(string id);
        Task<UserResponseViewModel> GetByEmail(string email);
        Task<UserResponseViewModel> GetByLoginAsync(string username, string hashedPassword);
        Task UpdateUser(UserEntity entity);
        Task DeleteUser(string id);
        UserResponseViewModel MapUserResponseFromEntity(UserEntity userEntity);
        Task<UserResponseViewModel> FindUserOrCreate(UserEntity userEntity);
    }
}
