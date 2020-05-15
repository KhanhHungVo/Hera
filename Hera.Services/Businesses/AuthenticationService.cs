using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class AuthenticationService : ServiceBaseTypeId<UserEntity, string>, IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenDescriptorOptions _tokenDescriptorOptions;

        public AuthenticationService(
            IUserRepository repository,
            IOptions<JwtTokenDescriptorOptions> tokenDescriptorOptions
        ) : base(repository)
        {
            _userRepository = repository;
            _tokenDescriptorOptions = tokenDescriptorOptions.Value;
        }

        public async Task<UserLoginRepsonseService> GetUserLogin(string username, string hashedPassword)
        {
            var query = _repository.QueryAsNoTracking()
                                   .Where(u => username.Equals(u.Username) &&
                                               hashedPassword.Equals(u.HashedPassword));
            var userEntity = await query.FirstOrDefaultAsync();

            return MapUserLoginResponseService(userEntity);
        }

        public async Task<UserRegisterViewModel> Register(UserRegisterViewModel userRegister)
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

            _repository.Add(userEntity);
            await _repository.SaveChangesAsync();

            return userRegister.WithoutPassword();
        }

        public async Task<JwtTokenViewModel> SignIn(string userName, string hashedPassword)
        {
            var user = await GetUserLogin(userName, hashedPassword);
            if (user == null)
            {
                return null;
            }
            var jwtSecurityToken = GenerateToken(user);

            await DeactivateOldUserToken(user.UserId);
            SaveUserToken(user.UserId, jwtSecurityToken);

            await _repository.SaveChangesAsync();

            return MapJwtTokenViewModel(jwtSecurityToken);
        }

        public JwtSecurityUserTokenViewModel GenerateToken(UserLoginRepsonseService user)
        {
            var claims = new List<Claim>
            {
                new Claim(HeraConstants.CLAIM_TYPE_USER_DATA, JsonConvert.SerializeObject(new UserCredentials {
                    // HeraConstants.CLAIM_TYPE_ROLES
                    Roles = new string[] { HeraConstants.CLAIM_HERA_USER },
                    Band = user.Band,
                    IsOnboarding = user.Onboarding,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.Email,
                    MobilePhone = user.Telephone
                }, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() })),
            };
            var accessToken = GenerateJwtSercurityAccessToken(claims);
            var refreshToken = GenerateJwtSercurityRefreshToken(claims);

            var jwtSecurityToken = new JwtSecurityUserTokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return jwtSecurityToken;
        }

        public async Task<JwtTokenViewModel> AcquireNewToken(UserCredentials userCredentials, JwtTokenViewModel jwtToken)
        {
            var userToken = await _repository.QueryAsNoTracking<UserTokenEntity, string>()
                                             .Where(ut =>
                                                ut.User.Email == userCredentials.EmailAddress &&
                                                ut.RefreshToken == jwtToken.RefreshToken &&
                                                ut.AccessToken == jwtToken.AccessToken &&
                                                ut.IsActive == true &&
                                                ut.IsDeleted == false
                                             )
                                             .Select(ut => new UserTokenEntity
                                             {
                                                 RefreshTokenExpiredTime = ut.RefreshTokenExpiredTime,
                                                 AccessTokenExpiredTime = ut.AccessTokenExpiredTime,
                                                 UserId = ut.UserId,
                                                 IsActive = ut.IsActive,
                                                 User = ut.User,
                                             })
                                             .FirstOrDefaultAsync();
            if (userToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            //if (DateTime.Now > userToken.RefreshTokenExpiredTime)
            //{
            //    throw new SecurityTokenException("This refresh token was expired");
            //}

            // userToken.RefreshTokenExpiredTime has more 5 minutes to live after accessToken was expired
            // we check accessToken has expired time offset before 5 minutes. In during time, we allow user to acquire new access token
            if ((DateTime.Now - userToken.AccessTokenExpiredTime).Minutes < -5)
            {
                throw new SecurityTokenException("You can use this access token until it's expired, bro");
            }

            var userLoginResponse = MapUserLoginResponseService(userToken.User);

            var jwtSecurityToken = GenerateToken(userLoginResponse);

            // we don't use this userTokenEntity to deactivate this
            // because we don't know if there are remaining userToken are activate (maybe someone did fishy user's refreshToken)
            // so deactivate all ones by user id
            await DeactivateOldUserToken(userToken.UserId);
            SaveUserToken(userToken.UserId, jwtSecurityToken);

            await _repository.SaveChangesAsync();

            return MapJwtTokenViewModel(jwtSecurityToken);
        }

        public async Task DeactivateOldUserToken(string userId)
        {
            var tokens = await _userRepository.Query<UserTokenEntity, string>()
                                              .Where(ut => ut.UserId == userId &&
                                                            ut.IsActive == true)
                                              .ToListAsync();
            if (tokens == null || !tokens.Any()) return;

            foreach (var token in tokens)
            {
                token.IsActive = false;
                _userRepository.SetEntityPropertiesHasModified<UserTokenEntity, string>(token, new string[] { nameof(UserTokenEntity.IsActive) });
            }
        }

        private UserLoginRepsonseService MapUserLoginResponseService(UserEntity userEntity)
        {
            return userEntity == null ? null : new UserLoginRepsonseService
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

        private JwtSecurityToken GenerateJwtSercurityAccessToken(IList<Claim> claims)
        {
            return GenerateJwtSecurityToken(claims, _tokenDescriptorOptions.AccessTokenExpiredTimeInMinute);
        }

        private JwtSecurityToken GenerateJwtSercurityRefreshToken(IList<Claim> claims)
        {
            var randomNumber = new byte[32];
            string randomIdentifiedRefreshToken = null;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                randomIdentifiedRefreshToken = Convert.ToBase64String(randomNumber).Replace("=", string.Empty);
            }

            claims.Add(new Claim(HeraConstants.CLAIM_TYPE_IDENTIFIED_R, randomIdentifiedRefreshToken));
            return GenerateJwtSecurityToken(claims, _tokenDescriptorOptions.RefreshTokenExpiredTimeInMinute);
        }

        private JwtSecurityToken GenerateJwtSecurityToken(IList<Claim> claims, int expiredFromNowToMinutes)
        {
            var signatureKey = Encoding.ASCII.GetBytes(_tokenDescriptorOptions.OAuthSignatureKey);
            return new JwtSecurityToken(
               issuer: _tokenDescriptorOptions.Issuer,
               audience: _tokenDescriptorOptions.Audience,
               notBefore: DateTime.UtcNow,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(expiredFromNowToMinutes),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signatureKey), SecurityAlgorithms.HmacSha256Signature)
           );
        }

        private JwtTokenViewModel MapJwtTokenViewModel(JwtSecurityUserTokenViewModel jwtSecurity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = new JwtTokenViewModel
            {
                AccessToken = tokenHandler.WriteToken(jwtSecurity.AccessToken),
                RefreshToken = tokenHandler.WriteToken(jwtSecurity.RefreshToken)
            };

            return jwtSecurityToken;
        }

        private void SaveUserToken(string userId, JwtSecurityUserTokenViewModel jwtSecurityUserToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var userToken = new UserTokenEntity
                {
                    AccessToken = tokenHandler.WriteToken(jwtSecurityUserToken.AccessToken),
                    AccessTokenIssuedTime = jwtSecurityUserToken.AccessToken.ValidFrom,
                    AccessTokenExpiredTime = jwtSecurityUserToken.AccessToken.ValidTo,

                    RefreshToken = tokenHandler.WriteToken(jwtSecurityUserToken.RefreshToken),
                    RefreshTokenIssuedTime = jwtSecurityUserToken.RefreshToken.ValidFrom,
                    RefreshTokenExpiredTime = jwtSecurityUserToken.RefreshToken.ValidTo,
                    UserId = userId
                };

                _userRepository.Add<UserTokenEntity, string>(userToken);                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ValidatedRegisterResult> ValidateEmail(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

            if (!regex.IsMatch(email))
            {
                return await Task.FromResult(new ValidatedRegisterResult
                {
                    IsValid = false,
                    Message = "Email is not valid format"
                });
            }

            var user = await _userRepository.QueryAsNoTracking().Where(u => u.Email == email).Select(u => u.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return await Task.FromResult(new ValidatedRegisterResult
                {
                    IsValid = true,
                    Message = ""
                });
            }

            return await Task.FromResult(new ValidatedRegisterResult
            {
                IsValid = false,
                Message = "This email was already existed"
            });
        }

        public async Task<ValidatedRegisterResult> ValidatePhoneNumber(string phoneNumber)
        {
            var user = await _userRepository.QueryAsNoTracking().Where(u => u.PhoneNumber == phoneNumber).Select(u => u.PhoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                return await Task.FromResult(new ValidatedRegisterResult
                {
                    IsValid = true,
                    Message = ""
                });
            }

            return await Task.FromResult(new ValidatedRegisterResult
            {
                IsValid = false,
                Message = "This telephone was already existed"
            });
        }

        public async Task<JwtTokenViewModel> AuthenticateWithGoogle(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            var user = await this.FindUserOrAdd(payload);
            var jwtSecurityToken = GenerateToken(user);

            await DeactivateOldUserToken(user.UserId);
            SaveUserToken(user.UserId, jwtSecurityToken);

            await _repository.SaveChangesAsync();

            return MapJwtTokenViewModel(jwtSecurityToken);
        }
        private async Task<UserLoginRepsonseService> FindUserOrAdd(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            var user = await GetUserByEmail(payload.Email);
            if (user == null)
            {
                var userEntity = new UserEntity
                {
                    Username = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    //Age = userRegister.Age,
                    //Band = userRegister.Band,
                    Email = payload.Email
                };
                _repository.Add(userEntity);
                await _repository.SaveChangesAsync();
                user = MapUserLoginResponseService(userEntity);
            }
            return user;
        }
        public async Task<UserLoginRepsonseService> GetUserByEmail(string email)
        {
            var query = _repository.QueryAsNoTracking()
                                   .Where(u => u.Email == email);
            var userEntity = await query.FirstOrDefaultAsync();

            return MapUserLoginResponseService(userEntity);
        }
    }
}
