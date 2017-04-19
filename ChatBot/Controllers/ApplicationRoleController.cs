using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Data.Infrastructure;
using ChatBot.Infrastructure.Core;
using ChatBot.Infrastructure.Extensions;
using ChatBot.Infrastructure.Mappings;
using ChatBot.Model.Models;
using ChatBot.Service;
using ChatBot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
  //  [Authorize]
    public class ApplicationRoleController : Controller
    {
        private IApplicationRoleService _appRoleService;
        private readonly ILoggingRepository _loggingRepository;
        private IBotDomainService _botDomainService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationRoleController(ILoggingRepository loggingRepository,IApplicationRoleService appRoleService, IBotDomainService botDomainService, RoleManager<IdentityRole> roleManager)
        {
            _appRoleService = appRoleService;
            _botDomainService = botDomainService;
            _roleManager = roleManager;
            _loggingRepository = loggingRepository;
        }
        //private int _page = 1;
        //private int _pageSize = 10;
        [HttpGet]
        public IEnumerable<ApplicationRoleViewModel> Get()
        {
            //var pagination = Request.Headers["Pagination"];

            //if (!string.IsNullOrEmpty(pagination))
            //{
            //    string[] vals = pagination.ToString().Split(',');
            //    int.TryParse(vals[0], out _page);
            //    int.TryParse(vals[1], out _pageSize);
            //}

            var result = _appRoleService.GetAll();
            //int currentPage = _page;
            //int currentPageSize = _pageSize;

            //var totalRecord = result.Count();
            //var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);
            //var resultPage = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            //Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            //    IEnumerable<ListdomainObject> listPagedDomain = Mapper.Map<IEnumerable<ListdomainObject>, IEnumerable<ListdomainObject>>(domains);
            var model = ViewModelMapper<ApplicationRoleViewModel, IdentityRole>.MapObjects(result.ToList(), null);
            return model;
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public ApplicationRoleViewModel Details(string id)
        {


            IdentityRole appRole = _appRoleService.GetDetail(id);
      
            if (appRole == null)
            {
                return null;
            }
            //  var appGroupViewModel = ViewModelMapper<ApplicationGroupViewModel, ApplicationGroup>(appGroup);
            var appGroupViewModel = PropertyCopy.Copy<ApplicationRoleViewModel, IdentityRole>(appRole);
        
            return appGroupViewModel;

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IActionResult result = new ObjectResult(false);
            GenericResult addResult = null;
            try
            {

                // var newAppRole = new ApplicationRole();
                // newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                //_appRoleService.Add(newAppRole);
                // _appRoleService.Save();

                var adminRole = await _roleManager.FindByNameAsync(applicationRoleViewModel.Name);
                if (adminRole == null)
                {
                    adminRole = new IdentityRole(applicationRoleViewModel.Name);
                    await _roleManager.CreateAsync(adminRole);
                }


                addResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Add removed."
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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IActionResult result = new ObjectResult(false);
            GenericResult updateResult = null;

         //   var appRole = _appRoleService.GetDetail(applicationRoleViewModel.Id);

            try
            {

             //   appRole.UpdateApplicationRole(applicationRoleViewModel, "update");

                var adminRole = await _roleManager.FindByIdAsync(applicationRoleViewModel.Id);
                adminRole.UpdateApplicationRole(applicationRoleViewModel, "update");

                if (adminRole != null)
                {
                  //  adminRole = new IdentityRole(applicationRoleViewModel.Name);
                    await _roleManager.UpdateAsync(adminRole);
                }


                //_appRoleService.Update(appRole);
                //_appRoleService.Save();

                updateResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Cập nhật role thành công"
                };

            }
            catch (Exception ex)
            {
                updateResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Tên không được trùng"
                };
                _loggingRepository.Add(new Error() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                _loggingRepository.Commit();
            }

            result = new ObjectResult(updateResult);
            return result;
        }


        //  [HttpDelete("{id:int}")]
        public IActionResult Delete(string id)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _removeResult = null;

            try
            {
                _appRoleService.Delete(id);
                _appRoleService.Save();

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
