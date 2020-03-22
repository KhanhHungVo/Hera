using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface IAuthenticationService : IServiceBaseTypeId<UserEntity, string>
    {
        Task<UserLoginRepsonseService> GetUserLogin(string username, string hashedPassword);
        Task<UserRegisterViewModel> Register(UserRegisterViewModel userRegister);
    }
}
