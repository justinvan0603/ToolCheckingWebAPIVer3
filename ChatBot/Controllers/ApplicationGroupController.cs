using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class ApplicationGroupController : Controller
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
        public ApplicationGroupController(
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
        //private int _page = 1;
        //private int _pageSize = 10;


        [HttpGet]
        [Authorize(Roles = "ViewUserGroup")]
        public IEnumerable<ApplicationGroupViewModel> Get()
        {
            //var pagination = Request.Headers["Pagination"];

            //if (!string.IsNullOrEmpty(pagination))
            //{
            //    string[] vals = pagination.ToString().Split(',');
            //    int.TryParse(vals[0], out _page);
            //    int.TryParse(vals[1], out _pageSize);
            //}

            var result = _appGroupService.GetAll();
            //int currentPage = _page;
            //int currentPageSize = _pageSize;

            //var totalRecord = result.Count();
            //var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);
            //var resultPage = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            //Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            var model = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(result);

            // var model = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>.MapObjects(resultPage.ToList(), _appRoleService);

            return model;
        }
        [HttpGet("{searchstring=}")]
        [Authorize(Roles = "ViewUserGroup")]
        public IEnumerable<ApplicationGroupViewModel> Get(string searchstring= null)
        {
         
            var result = _appGroupService.GetAll();
            if(!String.IsNullOrEmpty(searchstring))
            {
                result = result.Where(group => group.Name.ToLower().Contains(searchstring.ToLower()));
            }
            var model = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(result);


            return model.ToList();
        }
        [Route("detail/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewUserGroup")]
        public IEnumerable<ApplicationRoleViewModel> Details(int id)
        {

            ApplicationGroup appGroup = _appGroupService.GetDetail(id);
            if (appGroup == null)
            {
                return null;
            }
            //  var appGroupViewModel = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>(appGroup);
            var appGroupViewModel = PropertyCopy.Copy<ApplicationGroupViewModel, ApplicationGroup> (appGroup);

        

            var listRoleByGroup = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID).ToList();
            var listRoleByGroupViewModel = ViewModelMapper<ApplicationRoleViewModel, IdentityRole>.MapObjects(listRoleByGroup, null);

            var listRole = _appRoleService.GetAll().ToList();
            var listRoleViewModel = ViewModelMapper<ApplicationRoleViewModel, IdentityRole>.MapObjects(listRole, null);

                foreach (var roleViewModel in listRoleViewModel)
                {
                    foreach (var roleByGroupViewModel in listRoleByGroupViewModel)
                    {
                        if (roleByGroupViewModel.Id.Equals(roleViewModel.Id))
                        {
                        roleViewModel.Check = true;
                        break;
                        }
                    }
                }

            return listRoleViewModel;

        }


        [HttpPost]
        [Authorize(Roles = "AddUserGroup")]
        public IActionResult Create([FromBody]ApplicationGroupViewModel appGroupViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IActionResult result = new ObjectResult(false);
            GenericResult addResult = null;
            try
            {
                var newAppGroup = new ApplicationGroup();
                newAppGroup.Name = appGroupViewModel.Name;
                var appGroup = _appGroupService.Add(newAppGroup);
                _appGroupService.Save();

                //save group
                var listRoleGroup = new List<ApplicationRoleGroup>();
                var roles = appGroupViewModel.Roles.Where(x => x.Check).ToList();
                foreach (var role in roles)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = appGroup.Entity.ID,
                        RoleId = role.Id
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.Entity.ID);
                _appRoleService.Save();


                addResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Thêm group thành công"
                };
            }
            catch (Exception ex)
            {
                addResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            result = new ObjectResult(addResult);
            return result;
        }

        [HttpPut("{id}")]
   //     [Authorize(Roles = "EditUserGroup")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]ApplicationGroupViewModel appGroupViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IActionResult result = new ObjectResult(false);
            GenericResult genericResult = null;

            var listRoleRemoveList = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID).ToList();
       //    List<ApplicationRoleViewModel> listRoleRemoveList2 = Mapper.Map<IEnumerable<IdentityRole>, IEnumerable<ApplicationRoleViewModel>>(listRoleRemoveList).ToList();

            var appGroup = _appGroupService.GetDetail(appGroupViewModel.ID);
            try
            {
                appGroup.UpdateApplicationGroup(appGroupViewModel);
            //    appGroup = PropertyCopy.Copy<ApplicationGroup, ApplicationGroupViewModel>(appGroupViewModel);
                _appGroupService.Update(appGroup);
                _appGroupService.Save();

                //save group
                var listRoleGroup = new List<ApplicationRoleGroup>();

                var roleByUser = appGroupViewModel.Roles.Where(x => x.Check).ToList();
                foreach (var role in roleByUser)
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = appGroup.ID,
                        RoleId = role.Id
                    });
                }

                //add role to user
                var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID).ToList();

                //Xóa tất cả role thuộc group
                var listRoleRemoveName = listRoleRemoveList.Select(x => x.Name).ToArray();
                foreach (var user2 in listUserInGroup)
                {
                    foreach (var roleName in listRoleRemoveName)
                    {
                        await _userManager.RemoveFromRoleAsync(user2, roleName);
                    }
                }

                _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);

                _appRoleService.Save();

                var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID).ToList();

                var listRoleName = listRole.Select(x => x.Name).ToArray();
                foreach (var user in listUserInGroup)
                {
                    foreach (var roleName in listRoleName)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }


                genericResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Thêm group thành công"
                };


            }
            catch (Exception ex)
            {
                genericResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Thêm group thất bại"
                };
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }


            result = new ObjectResult(genericResult);
            return result;
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "DeleteUserGroup")]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _removeResult = null;

            try
            {


                var listUserInGroup = _appGroupService.GetListUserByGroupId(id).ToList();
                var listRole = _appRoleService.GetListRoleByGroupId(id).ToList();

                foreach (var userinGroup in listUserInGroup)
                {
                    foreach (var roleName in listRole)
                    {

                        //if (!await _userManager.IsInRoleAsync(user, roleName))
                        //{

                        await _userManager.RemoveFromRoleAsync(userinGroup, roleName.Name);
                        //     }
                    }
                }

                var appGroup = _appGroupService.Delete(id);
                _appGroupService.Save();

               

                _removeResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Domain removed."
                };
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



    }
}
