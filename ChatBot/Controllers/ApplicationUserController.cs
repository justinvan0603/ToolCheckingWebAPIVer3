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
using System.Xml.Linq;
using AutoMapper;
using ChatBot.Data.Infrastructure;
using ChatBot.Infrastructure.Core;
using ChatBot.Infrastructure.Extensions;
using ChatBot.Infrastructure.Mappings;
using ChatBot.Infrastructure.MD5Encryption;
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
        private readonly DEFACEWEBSITEContext _context;

        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly JwtIssuerOptions _jwtOptions;
        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            ILoggerFactory loggerFactory, IOptions<JwtIssuerOptions> jwtOptions, RoleManager<IdentityRole> roleManager, ILoggingRepository loggingRepository, IApplicationGroupService appGroupService, IApplicationRoleService appRoleService, DEFACEWEBSITEContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _loggingRepository = loggingRepository;
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _context = context;


            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _jwtOptions = jwtOptions.Value;
        }
        //private int _page = 1;
        //private int _pageSize = 10;


        [HttpGet]
     //   [Authorize(Roles = "ViewUser")]
     //   [Authorize("ViewUser")]
        public IEnumerable<ApplicationUserViewModel> Get()
        {
            
            //var pagination = Request.Headers["Pagination"];

            //if (!string.IsNullOrEmpty(pagination))
            //{
            //    string[] vals = pagination.ToString().Split(',');
            //    int.TryParse(vals[0], out _page);
            //    int.TryParse(vals[1], out _pageSize);
            //}
            var userRoot = _userManager.GetUserId(User);
            var result = _userManager.Users.ToList().FindAll(x => x.LockoutEnabled &&x.PARENT_ID==userRoot);
        //    var s = result.FindAll(x => x.LockoutEnabled);


            //int currentPage = _page;
            //int currentPageSize = _pageSize;

            //var totalRecord = result.Count();
            //var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            //var domains = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            //Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            var model = ViewModelMapper<ApplicationUserViewModel, ApplicationUser>.MapObjects(result.ToList(), null);
           
            return model;
        }
        [HttpGet("{searchstring=}")]
        public IEnumerable<ApplicationUserViewModel> Get(string searchstring = null)
        {



            var result = _userManager.Users; ;
            if (!String.IsNullOrEmpty(searchstring))
            {
                result = result.Where(user => user.FULLNAME.ToLower().Contains(searchstring.ToLower()) ||
                                                user.Email.ToLower().Contains(searchstring.ToLower()) ||
                                                user.UserName.ToLower().Contains(searchstring.ToLower()) ||
                                                user.PHONE.ToString().Contains(searchstring.ToLower()));
            }
            var model = ViewModelMapper<ApplicationUserViewModel, ApplicationUser>.MapObjects(result.ToList(), null);

            return model.ToList();
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
     //   [Authorize(Roles = "ViewUser")]
        public async Task<IActionResult> Create([FromBody]ApplicationUserViewModel applicationUserViewModel)
        {
            ////Thêm quyền
        



        //    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.update"));


            //       var newAppUser = new ApplicationUser();
            //  newAppUser.UpdateUser(applicationUserViewModel);
            //  ApplicationUser newAppUser = PropertyCopy.Copy<ApplicationUser, ApplicationUserViewModel>(applicationUserViewModel);


            IActionResult actionResult = new ObjectResult(false);
            GenericResult addResult = null;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ga");
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
                    actionResult = new ObjectResult(addResult);
                    return actionResult;
                }
                var userByUserName = await _userManager.FindByNameAsync(applicationUserViewModel.UserName);
                if (userByUserName != null)
                {
                    addResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Username đã tồn tại"
                    };
                    actionResult = new ObjectResult(addResult);
                    return actionResult;
                }


                ApplicationUser newAppUser = Mapper.Map<ApplicationUserViewModel, ApplicationUser>(applicationUserViewModel);
                newAppUser.Id = Guid.NewGuid().ToString();
                newAppUser.PARENT_ID = null;
                newAppUser.RECORD_STATUS = "1";
                newAppUser.AUTH_STATUS = "U";
                newAppUser.APPROVE_DT = null;
                newAppUser.EDIT_DT = null;
                newAppUser.PASSWORD = null;
                newAppUser.CREATE_DT = DateTime.Now.Date;
                newAppUser.PARENT_ID = _userManager.GetUserId(User);

                var result = await _userManager.CreateAsync(newAppUser, applicationUserViewModel.PASSWORD);
               
                if (result.Succeeded)
                {


                    var listAppUserGroup = new List<ApplicationUserGroup>();
                    var groups = applicationUserViewModel.Groups.Where(xy => xy.Check).ToList();
                    foreach (var group in groups)
                    {
                        listAppUserGroup.Add(new ApplicationUserGroup()
                        {
                            GroupId = group.ID,
                            UserId = newAppUser.Id
                        });

                        var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();

                        List<string> list = new List<string>();
                        foreach (var role in listRole)
                        {
                            list.Add(role.Name);

                        }
                        foreach (var item in list)
                        {
                               await _userManager.RemoveFromRoleAsync(newAppUser, item);
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

               
                    //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
                    //string pass = MD5Encoder.MD5Hash(user.Password);
                    XElement xmldata = new XElement(new XElement("Root"));
                    XElement x = new XElement("Domain", new XElement("DOMAIN", applicationUserViewModel.Domain),
                                                       new XElement("DESCRIPTION", applicationUserViewModel.DomainDesc));
                    xmldata.Add(x);

                    string command = $"dbo.Users_Ins @p_USERNAME = '{newAppUser.UserName}', @p_FULLNAME= N'{newAppUser.FULLNAME}',@p_PASSWORD = '{null}',@p_EMAIL = '{newAppUser.Email}',@p_PHONE = {newAppUser.PHONE},@p_PARENT_ID = '',@p_DESCRIPTION = N'{newAppUser.DESCRIPTION}',@p_RECORD_STATUS = '{newAppUser.RECORD_STATUS}',@p_AUTH_STATUS = '{newAppUser.AUTH_STATUS}',@p_CREATE_DT = '{DateTime.Now.Date}',@p_APPROVE_DT = '{newAppUser.APPROVE_DT}' ,@p_EDIT_DT= '{newAppUser.EDIT_DT}' ,@p_MAKER_ID ='{newAppUser.MAKER_ID}',@p_CHECKER_ID = '{newAppUser.CHECKER_ID}',@p_EDITOR_ID = '{newAppUser.EDITOR_ID}',@DOMAIN ='{xmldata}'";
                    var resultStore = _context.Database.ExecuteSqlCommand(command);
                    if (resultStore == -1)
                    {
                        addResult = new GenericResult()
                        {
                            Succeeded = false,
                            Message = "Thêm domain thất bại"
                        };
                    }

                    addResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Thêm dữ liệu thành công"
                    };


                }
                else
                {
                    addResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Mật khẩu đơn giản (Hãy thử lại với chữ, số, ký tự đặc biệt)"
                    };
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
      //          ApplicationUser appUser = Mapper.Map<ApplicationUserViewModel, ApplicationUser>(applicationUserViewModel);
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    var listAppUserGroup = new List<ApplicationUserGroup>();
                    var applicationGroupCheckViewModel = applicationUserViewModel.Groups.Where(x => x.Check).ToList();
                    foreach (var group in applicationGroupCheckViewModel)
                    {
                        listAppUserGroup.Add(new ApplicationUserGroup()
                        {
                            GroupId = group.ID,
                            UserId = applicationUserViewModel.Id
                        });
     
                        var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();
                        //List<string> list = new List<string>();
                        //foreach (var role in listRole)
                        //{
                        //    list.Add(role.Name);

                        //}
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var item in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, item);

                            IdentityResult result2 = await _userManager.AddToRoleAsync(appUser, item);
                            if (!result2.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                    }
                    var applicationGroupUnCheckViewModel = applicationUserViewModel.Groups.Where(x => x.Check == false);
                    foreach (var group in applicationGroupUnCheckViewModel)
                    {
                        var listRole = _appRoleService.GetListRoleByGroupId(group.ID).ToList();
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var item in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, item);
                        }
                    }

                    _appGroupService.AddUserToGroups(listAppUserGroup, applicationUserViewModel.Id);
                    _appGroupService.Save();
                    addResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Cập nhật thành công"
                    };
                }

            }
            catch (Exception ex)
            {
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
                addResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Dữ liệu không hợp lệ"
                };

            }

            actionresult = new ObjectResult(addResult);
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
                // var result = await _userManager.DeleteAsync(appUser);

                var lock2 =  await _userManager.SetLockoutEnabledAsync(appUser, false);
                int a = 5;
        
                    // user is locked out; take appropriate action
                    _removeResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Xóa thành công"
                    };
                //if (result.Succeeded)
                //{
                //    _removeResult = new GenericResult()
                //    {
                //        Succeeded = true,
                //        Message = "Xóa thành công"
                //    };
                //}

                 
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

        public IActionResult Ga()
        {
            return Unauthorized();
        }
    }
}
