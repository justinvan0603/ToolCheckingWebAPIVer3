using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.Data.Infrastructure;
using ChatBot.Infrastructure.Core;
using ChatBot.Model.Models;
using ChatBot.Models;
using ChatBot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggingRepository _loggingRepository;
        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly JwtIssuerOptions _jwtOptions;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            ILoggerFactory loggerFactory, IOptions<JwtIssuerOptions> jwtOptions, RoleManager<IdentityRole> roleManager, ILoggingRepository loggingRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _loggingRepository = loggingRepository;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _jwtOptions = jwtOptions.Value;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;
            
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var result = await _userManager.CheckPasswordAsync(user, model.Password);

                if (result)
                {
                    var principal = await _signInManager.CreateUserPrincipalAsync(user);

                    // Create the JWT security token and encode it.
                    var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        claims: principal.Claims,
                        notBefore: _jwtOptions.NotBefore,
                        expires: _jwtOptions.Expiration,
                        signingCredentials: _jwtOptions.SigningCredentials);

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    // Serialize and return the response
                    var response = new
                    {
                        access_token = encodedJwt,
                        expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                    };

                    _authenticationResult = new GenericTokenResult()
                    {
                        Succeeded = true,
                        Message = "Authentication succeeded",
                        access_token = encodedJwt,
                        expires_in = (int)_jwtOptions.ValidFor.TotalSeconds

                    };
                }
                else
                {
                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Authentication failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _authenticationResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            _result = new ObjectResult(_authenticationResult);
            return _result;
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
                return BadRequest();
            }

        }

    }
}
