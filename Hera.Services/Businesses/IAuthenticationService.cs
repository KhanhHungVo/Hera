using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Common.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface IAuthenticationService : IServiceBaseTypeId<UserEntity, string>
    {
        Task<JwtTokenViewModel> SignIn(string userName, string hashedPassword);
        Task<UserViewModel> Register(UserRegisterViewModel userRegister);
        JwtSecurityUserTokenViewModel GenerateToken(UserViewModel userLogin);
        Task<JwtTokenViewModel> AcquireNewToken(UserCredentials userCredentials, JwtTokenViewModel jwtToken);
        Task DeactivateOldUserToken(string userId);
        Task<ValidatedRegisterResult> ValidateEmail(string email);
        Task<ValidatedRegisterResult> ValidatePhoneNumber(string email);
        Task<JwtTokenViewModel> AuthenticateWithGoogle(SocialUserInfo ggUserInfo);
        Task<JwtTokenViewModel> AuthenticateWithFacebook(SocialUserInfo fbUserInfo);
        Task<string> ValidateUserDefinedRules(UserRegisterViewModel userRegister);
    }
}
