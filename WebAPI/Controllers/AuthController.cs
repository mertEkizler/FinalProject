using Business.Abstract;
using Castle.Core.Logging;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, ILogger logger)
        {
            _logger = logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            const string methodName = nameof(Login);

            _logger.Trace($"[{methodName}] Invoked.");

            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            _logger.Trace($"[{methodName}] Creating access token...");
            var result = _authService.CreateAccessToken(userToLogin.Data);

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning results...");
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            const string methodName = nameof(Register);

            _logger.Trace($"[{methodName}] Invoked.");

            _logger.Trace($"[{methodName}] Checking user with email.");
            var userExists = _authService.UserExists(userForRegisterDto.Email);

            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            _logger.Trace($"[{methodName}] Creating access token...");
            var result = _authService.CreateAccessToken(registerResult.Data);

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning results...");
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}