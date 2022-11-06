
using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data.Dtos;
using HotelListing.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;
        private const string _loginProvider = "HotelListingAPI";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> Login(UserLoginDto userLoginDto)
        {
            _user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (_user is null)
                return null;
            if (!(await _userManager.CheckPasswordAsync(_user, userLoginDto.Password)))
                return null;

            return new AuthResponseDto { UserID = _user.Id, Token = await _GenerateToken(), RefreshToken = await CreateRefreshToken() };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserRegisterDto userRegisterDto)
        {
            _user = _mapper.Map<ApiUser>(userRegisterDto);
            _user.UserName = userRegisterDto.Email;

            var result = await _userManager.CreateAsync(_user, userRegisterDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = tokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            _user = await _userManager.FindByNameAsync(username);
            if (_user is null || _user.Id != request.UserID)
                return null;

            var isValidToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.Token);
            if (!isValidToken)
            {
                await _userManager.UpdateSecurityStampAsync(_user);
                return null;
            }

            var token = await _GenerateToken();
            return new AuthResponseDto
            {
                UserID = _user.Id,
                Token = token,
                RefreshToken = await CreateRefreshToken()
            };
        }

        private async Task<string> _GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var cresentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email)

            }.Union(roleClaims).Union(userClaims);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWTSettings:Issuer"],
                    audience: _configuration["JWTSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWTSettings:DurationInHours"])),
                    signingCredentials: cresentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
