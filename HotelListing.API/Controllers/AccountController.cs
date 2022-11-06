using HotelListing.API.Contracts;
using HotelListing.API.Data.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody]UserRegisterDto UserRegisterDto)
        {
            _logger.LogInformation($"Registration Attempt for {UserRegisterDto.Email}");

            var errors = await _authManager.Register(UserRegisterDto);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto UserLoginDto)
        {
            _logger.LogInformation($"Login Attempt for {UserLoginDto.Email}");

            var authResponse = await _authManager.Login(UserLoginDto);
            if (authResponse is null)
                return Unauthorized();
            return Ok(authResponse);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);
            if (authResponse is null)
                return Unauthorized();
            return Ok(authResponse);
        }
    }
}
