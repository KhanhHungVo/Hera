using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface IAuthenticationService : IServiceBaseTypeId<UserEntity, string>
    {
        Task<UserLoginRepsonseService> GetUserLogin(string username, string hashedPassword);
        Task<JwtTokenViewModel> SignIn(string userName, string hashedPassword);
        Task<UserRegisterViewModel> Register(UserRegisterViewModel userRegister);
        JwtSecurityUserTokenViewModel GenerateToken(UserLoginRepsonseService userLogin);
        Task<JwtTokenViewModel> AcquireNewToken(UserCredentials userCredentials, JwtTokenViewModel jwtToken);
        Task DeactivateOldUserToken(string userId);
        Task<ValidatedRegisterResult> ValidateEmail(string email);
        Task<ValidatedRegisterResult> ValidatePhoneNumber(string email);
        Task<JwtTokenViewModel> AuthenticateWithGoogle(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
    }
}
