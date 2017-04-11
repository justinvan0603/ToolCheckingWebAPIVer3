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


        //private readonly IMembershipService _membershipService;
        //private readonly IUserRepository _userRepository;


        //public AccountController(IMembershipService membershipService,
        //    IUserRepository userRepository,
        //    ILoggingRepository _errorRepository)
        //{
        //    _membershipService = membershipService;
        //    _userRepository = userRepository;
        //    _loggingRepository = _errorRepository;
        //}

        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user == null)
        //    {
        //        _authenticationResult = new GenericResult()
        //        {
        //            Succeeded = true,
        //            Message = "Authentication succeeded"
        //        };
        //    }


        //    throw new NotImplementedException();
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;
            _authenticationResult = new GenericResult()
            {
                Succeeded = false,
                Message = "Authentication failed"
            };
            _result = new ObjectResult(_authenticationResult);
            return _result;
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


        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    try
        //    {
        //        await HttpContext.Authentication.SignOutAsync("Cookies");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //     //   _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
        //     //   _loggingRepository.Commit();

        //        return BadRequest();
        //    }

        //}



        //[Route("register")]
        //[HttpPost]
        //public IActionResult Register([FromBody] RegistrationViewModel user)
        //{
        //    IActionResult _result = new ObjectResult(false);
        //    GenericResult _registrationResult = null;

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            User _user = _membershipService.CreateUser(user.Username, user.Email, user.Password, new int[] { 1 });

        //            if (_user != null)
        //            {
        //                _registrationResult = new GenericResult()
        //                {
        //                    Succeeded = true,
        //                    Message = "Registration succeeded"
        //                };
        //            }
        //        }
        //        else
        //        {
        //            _registrationResult = new GenericResult()
        //            {
        //                Succeeded = false,
        //                Message = "Invalid fields."
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _registrationResult = new GenericResult()
        //        {
        //            Succeeded = false,
        //            Message = ex.Message
        //        };

        //        _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
        //        _loggingRepository.Commit();
        //    }

        //    _result = new ObjectResult(_registrationResult);
        //    return _result;
        //}


        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            //Thêm quyền
            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            var managerRole = await _roleManager.FindByNameAsync("Manager");
            if (managerRole == null)
            {
                managerRole = new IdentityRole("Manager");
                await _roleManager.CreateAsync(managerRole);
            }

            //var result = await _userManager.CreateAsync(
            //    new ApplicationUser
            //    {
            //        UserName = model.Username,
            //        Email = model.Email,
            //        CreateDt = DateTime.Now,
            //        ApproveDt = null,
            //        EditDt = null
            //    },
            //    model.Password);


            //var adminUser = _userManager.FindByEmailAsync("thieuvq@gmail.com");
            //await _userManager.AddToRoleAsync(await adminUser, "Admin");
            return Ok();

        }
    }
}
