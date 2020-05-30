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
        Task<UserViewModel> CreateUserAsync(UserRegisterViewModel userRegister);
        Task<IEnumerable<UserViewModel>> GetListAsync();
        Task<UserViewModel> GetById(string id);
        Task<UserViewModel> GetByEmail(string email);
        Task<UserViewModel> GetByLoginAsync(string username, string hashedPassword);
        Task<UserViewModel> UpdateUser(UserViewModel model);
        Task DeleteUser(string id);
        UserViewModel MapToViewModel(UserEntity userEntity);
        Task<UserViewModel> FindUserOrCreate(UserEntity userEntity);
    }
}
