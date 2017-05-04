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
using ChatBot.ViewModels.AccountViewModels;

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
            ThrowIfInvalidOptions(_jwtOptions);
        }


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {


            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;
            
            try
            {

             //   var user23 = _userManager.FindByNameAsync(model.Username).Result.Claims;

                var user = await _userManager.FindByNameAsync(model.Username);
                var result = await _userManager.CheckPasswordAsync(user, model.Password);

                if (result)
                {

                    //var adminRole = await _roleManager.FindByNameAsync("Admin");
                    //if (adminRole == null)
                    //{
                    //    adminRole = new IdentityRole("Admin");
                    //    await _roleManager.CreateAsync(adminRole);
                    //}
                    //await _roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, "projects.create"));


                    //var accountManagerRole = await _roleManager.FindByNameAsync("Account Manager");

                    //if (accountManagerRole == null)
                    //{
                    //    accountManagerRole = new IdentityRole("Account Manager");
                    //    await _roleManager.CreateAsync(accountManagerRole);

                    //    await _roleManager.AddClaimAsync(accountManagerRole, new Claim(, "account.manage"));
                    //}


                    var principal =  _signInManager.CreateUserPrincipalAsync(user).Result.Claims.ToList();
                    
                    var xfd = _userManager.GetClaimsAsync(user).Result;


                   // ClaimsIdentity claim = new ClaimsIdentity();
                   // _signInManager.
                    var now = DateTime.UtcNow;
                    var claims = new Claim[]
                {
                    
                        new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    

                };
                    var claimslist = claims.ToList();
                    foreach (var claim in principal)
                    {
                        claimslist.Add(claim);
                    };
               
                    // Create the JWT security token and encode it.
                    var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        claims: principal,
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


                    if (!String.IsNullOrEmpty(model.TokenFirebase))
                    {
                        user.APPTOKEN = model.TokenFirebase;
                        await _userManager.UpdateAsync(user);
                    }

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
                    _authenticationResult = new GenericTokenResult()
                    {
                        Succeeded = false,
                        Message = "Tài khoản hoặc mật khẩu không đúng",
                        access_token = null,
                        expires_in = 0
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


        private static long ToUnixEpochDate(DateTime date)
     => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
         

            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;

            try
            {
                ApplicationUser appUser=null;
                if (!String.IsNullOrEmpty(model.Id))
                {
                    appUser = await _userManager.FindByIdAsync(model.Id);

                }
                else if (!String.IsNullOrEmpty(model.UserName))
                {
                    appUser = await _userManager.FindByNameAsync(model.UserName);
                }
                //else
                //{
                //    appUser = await _userManager.FindByIdAsync(model.Id);
                //}

                if (appUser!=null)
                {
                    var result =
                   await
                      _userManager.ChangePasswordAsync(appUser, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {

                        _authenticationResult = new GenericResult()
                        {
                            Succeeded = true,
                            Message = "Đổi mật khẩu thành công",
                        };
                    }

                    else
                    {
                        _authenticationResult = new GenericResult()
                        {
                            Succeeded = false,
                            Message = "Đổi mật khẩu thất bại",
                        };
                    }
                
            }
                else
                {
                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Không tìm thấy ID",
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
                ApplicationUser userLogin = await _userManager.GetUserAsync(User);
                
              //  var principal = await _signInManager.SignInAsync();
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
