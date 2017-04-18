using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Data.Infrastructure;
using ChatBot.Infrastructure.Core;
using ChatBot.Infrastructure.Extensions;
using ChatBot.Infrastructure.Mappings;
using ChatBot.Model.Models;
using ChatBot.Models;
using ChatBot.Service;
using ChatBot.ViewModels;
using IdentityModel;
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
    public class ApplicationUserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggingRepository _loggingRepository;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;

        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly JwtIssuerOptions _jwtOptions;
        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            ILoggerFactory loggerFactory, IOptions<JwtIssuerOptions> jwtOptions, RoleManager<IdentityRole> roleManager, ILoggingRepository loggingRepository, IApplicationGroupService appGroupService, IApplicationRoleService appRoleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _loggingRepository = loggingRepository;
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;


            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _jwtOptions = jwtOptions.Value;
        }
        private int _page = 1;
        private int _pageSize = 10;


        [HttpGet]
     //   [Authorize(Roles = "ViewUser")]
     //   [Authorize("ViewUser")]
        public IEnumerable<ApplicationUserViewModel> Get()
        {
            
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            var result = _userManager.Users; ;
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var domains = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            var model = ViewModelMapper<ApplicationUserViewModel, ApplicationUser>.MapObjects(result.ToList(), null);

            return model;
        }


        [HttpGet("detail")]
        public IEnumerable<ApplicationGroupViewModel> Details(string id)
        {
            ;

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return null;
            }
            var user = _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }
            else
            {

                var applicationUserViewModel = PropertyCopy.Copy<ApplicationUserViewModel, ApplicationUser>(user.Result);
                var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
                var listGroupsViewModel = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>.MapObjects(listGroup.ToList(), null);


                var result = _appGroupService.GetAll().ToList();
                var model = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(result).ToList();

                foreach (var groupsByUserViewModel in listGroupsViewModel)
                {
                    foreach (var group in model)
                    {
                        if (groupsByUserViewModel.ID.Equals(group.ID))
                        {
                            group.Check = true;
                            break;
                        }
                    }
                }

                // applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                return model;
            }

        }

        //[HttpGet("detail")]
        //public IEnumerable<ApplicationGroupViewModel> Details(string id)
        //{
        //    ;

        //    if (string.IsNullOrEmpty(id.ToString()))
        //    {
        //        return null;
        //    }
        //    var user = _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var applicationUserViewModel = PropertyCopy.Copy<ApplicationUserViewModel, ApplicationUser>(user.Result);
        //        var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
        //        // applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
        //        var listGroupsViewModel = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>.MapObjects(listGroup.ToList(), null);
        //        return listGroupsViewModel;
        //    }

        //}



        [HttpPost]
        [Authorize(Roles = "ViewUser")]
        public async Task<IActionResult> Create([FromBody]ApplicationUserViewModel applicationUserViewModel)
        {
            ////Thêm quyền
            //var adminRole = await _roleManager.FindByNameAsync("Admin");
            //if (adminRole == null)
            //{
            //    adminRole = new IdentityRole("Admin");
            //    await _roleManager.CreateAsync(adminRole);
            //}
            //await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.create"));
            //await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.update"));
            //       var newAppUser = new ApplicationUser();
            //  newAppUser.UpdateUser(applicationUserViewModel);
            //  ApplicationUser newAppUser = PropertyCopy.Copy<ApplicationUser, ApplicationUserViewModel>(applicationUserViewModel);

            IActionResult actionResult = new ObjectResult(false);
            GenericResult addResult = null;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
        
            try
            {

                var userByEmail = await _userManager.FindByEmailAsync(applicationUserViewModel.Email);
                if (userByEmail != null)
                {
                    addResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Email đã tồn tại"
                    };
                }
                var userByUserName = await _userManager.FindByNameAsync(applicationUserViewModel.UserName);
                if (userByUserName != null)
                {
                    addResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Username đã tồn tại"
                    };
                }


                ApplicationUser newAppUser = Mapper.Map<ApplicationUserViewModel, ApplicationUser>(applicationUserViewModel);
                newAppUser.Id = Guid.NewGuid().ToString();

                var result = await _userManager.CreateAsync(newAppUser, newAppUser.Password);
               
                if (result.Succeeded)
                {


                    var listAppUserGroup = new List<ApplicationUserGroup>();
                    foreach (var group in applicationUserViewModel.Groups.Where(x=>x.Check))
                    {
                        listAppUserGroup.Add(new ApplicationUserGroup()
                        {
                            GroupId = group.ID,
                            UserId = newAppUser.Id
                        });

                        var listRole = _appRoleService.GetListRoleByGroupId(group.ID);

                        List<string> list = new List<string>();
                        foreach (var role in listRole)
                        {
                            list.Add(role.Name);

                        }
                        foreach (var item in list)
                        {
                            //   await _userManager.RemoveFromRoleAsync(newAppUser, item);
                            if (!await _userManager.IsInRoleAsync(newAppUser, item))
                            {
                                IdentityResult result2 = await _userManager.AddToRoleAsync(newAppUser, item);
                                if (!result2.Succeeded)
                                {
                                    AddErrorsFromResult(result);
                                 
                                }
                            }
                            
                        }

                    }

                    _appGroupService.AddUserToGroups(listAppUserGroup, newAppUser.Id);
                    _appGroupService.Save();


                }
            }
            catch (Exception ex)
            {
                addResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Tên không được trùng"
                };
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }
            actionResult = new ObjectResult(addResult);
            return actionResult;


        }




        [HttpPut]
        [Authorize(Roles = "EditUser")]
        public async Task<IActionResult> PutAsync([FromBody]ApplicationUserViewModel applicationUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IActionResult actionresult = new ObjectResult(false);
            GenericResult addResult = null;

            var appUser = await _userManager.FindByIdAsync(applicationUserViewModel.Id);


            try
            {
                appUser.UpdateUser(applicationUserViewModel);
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    var listAppUserGroup = new List<ApplicationUserGroup>();
                    foreach (var group in applicationUserViewModel.Groups.Where(x=>x.Check))
                    {
                        listAppUserGroup.Add(new ApplicationUserGroup()
                        {
                            GroupId = group.ID,
                            UserId = applicationUserViewModel.Id
                        });
     
                        var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();
                        IEnumerable<string> xff = new List<string>();
                        List<string> list = new List<string>();
                        foreach (var role in listRole)
                        {
                            list.Add(role.Name);

                        }
                        foreach (var item in list)
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, item);

                            IdentityResult result2 = await _userManager.AddToRoleAsync(appUser, item);
                            if (!result2.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                        //IdentityResult result2 = await _userManager.AddToRolesAsync(appUser, en);
                        //if (!result2.Succeeded)
                        //{
                        //    AddErrorsFromResult(result);
                        //}

                        //foreach (var role in listRole)
                        //{
                        //    await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                        //    await _userManager.AddToRoleAsync(appUser.Id, role.Name);
                        //}
                    }
                    _appGroupService.AddUserToGroups(listAppUserGroup, applicationUserViewModel.Id);
                    _appGroupService.Save();

                }

            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }


            return actionresult;
        }


        [HttpDelete]
        //[Authorize(Roles = "DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
         
            IActionResult _result = new ObjectResult(false);
            GenericResult _removeResult = null;

            try
            {
                var appUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    _removeResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Domain removed."
                    };
                }

                 
            }
            catch (Exception ex)
            {
                _removeResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            _result = new ObjectResult(_removeResult);
            return _result;
        }


        //[HttpGet("detail")]
        //public ApplicationUserViewModel Details(string id)
        //{
        //    ;

        //    if (string.IsNullOrEmpty(id.ToString()))
        //    {
        //        return null;
        //    }
        //    var user = _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var applicationUserViewModel = PropertyCopy.Copy<ApplicationUserViewModel, ApplicationUser>(user.Result);
        //        var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
        //        // applicationUserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
        //        applicationUserViewModel.Groups = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>.MapObjects(listGroup.ToList(), null);
        //        return applicationUserViewModel;
        //    }

        //}


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
