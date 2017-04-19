﻿using System;
using System.Collections.Generic;
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
        private int _page = 1;
        private int _pageSize = 10;


        [HttpGet]
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

        [Route("detail/{id:int}")]
        [HttpGet]
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
                foreach (var role in appGroupViewModel.Roles.Where(x=>x.Check))
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
        public async Task<IActionResult> PutAsync(int id, [FromBody]ApplicationGroupViewModel appGroupViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IActionResult result = new ObjectResult(false);
            GenericResult genericResult = null;


            var appGroup = _appGroupService.GetDetail(appGroupViewModel.ID);
            try
            {
                appGroup.UpdateApplicationGroup(appGroupViewModel);

            //    appGroup = PropertyCopy.Copy<ApplicationGroup, ApplicationGroupViewModel>(appGroupViewModel);
                _appGroupService.Update(appGroup);
                _appGroupService.Save();

                //save group
                var listRoleGroup = new List<ApplicationRoleGroup>();
                foreach (var role in appGroupViewModel.Roles.Where(x=>x.Check))
                {
                    listRoleGroup.Add(new ApplicationRoleGroup()
                    {
                        GroupId = appGroup.ID,
                        RoleId = role.Id
                    });
                }
                _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                _appRoleService.Save();

                //add role to user
                var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID).ToList();
                var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID);


                //var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();
                List<string> list = new List<string>();
                foreach (var role in listRole)
                {
                    list.Add(role.Name);

                }

                foreach (var user in listUserInGroup)
                {
                  //  var listRoleName = listRole.Select(x => x.Name).ToArray();
                    //foreach (var roleName in list)
                    //{
                        //if (!await _userManager.IsInRoleAsync(user, list))
                        //{
                            await _userManager.AddToRolesAsync(user, list);
                       // }
                //    }
                }

                genericResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Thêm group thành công"
                };


            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }


            result = new ObjectResult(genericResult);
            return result;
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _removeResult = null;

            try
            {
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