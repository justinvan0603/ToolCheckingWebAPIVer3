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
    public class MenuRoleController : Controller
    {
        private IApplicationRoleService _appRoleService;
        private readonly ILoggingRepository _loggingRepository;
        private IMenuRoleService _menuRoleService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public MenuRoleController(ILoggingRepository loggingRepository, IApplicationRoleService appRoleService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMenuRoleService menuRoleService)
        {
            _appRoleService = appRoleService;
            _roleManager = roleManager;
            _userManager = userManager;
            _menuRoleService = menuRoleService;
            _loggingRepository = loggingRepository;
        }

        List<MenuRole> listMenu1;
        List<MenuRole> listMenu2;
        List<MenuRole> listMenu3;
        //[HttpGet]
        //[Route("/getMenuJson")]
        [HttpGet("getMenuJson")]
        public async Task<List<MenuRoleJson.Rootobject>> GetMenuJson()
        {
           // var resual = "  [{\r\n      \"path\": \"pages222\",\r\n      \"children\": [\r\n        {\r\n          \"path\": \"messages\",\r\n          \"data\": {\r\n            \"menu\": {\r\n              \"title\": \"Thông báo\",\r\n              \"icon\": \"ion-android-notifications-none\",\r\n              \"selected\": false,\r\n              \"expanded\": false,\r\n              \"order\": 500\r\n            }\r\n          },\r\n          \"children\": [\r\n            {\r\n              \"path\": \"messagelist\",\r\n              \"data\": {\r\n                \"menu\": {\r\n                  \"title\": \"Danh sách thô\"\r\n                }\r\n              }\r\n            }\r\n          ]\r\n        }\r\n        ]\r\n  },\r\n  {\r\n      \"path\": \"pages\",\r\n      \"children\": [\r\n        {\r\n          \"path\": \"messages\",\r\n          \"data\": {\r\n            \"menu\": {\r\n              \"title\": \"Thông báo\",\r\n              \"icon\": \"ion-android-notifications-none\",\r\n              \"selected\": false,\r\n              \"expanded\": false,\r\n              \"order\": 500\r\n            }\r\n          },\r\n          \"children\": [\r\n            {\r\n              \"path\": \"messagelist\",\r\n              \"data\": {\r\n                \"menu\": {\r\n                  \"title\": \"Danh sách thông báo\"\r\n                }\r\n              }\r\n            }\r\n          ]\r\n        }\r\n        ]\r\n  }\r\n]\r\n\r\n    \r\n";

            //     var x = User.;
            //  var result = _appRoleService.GetAll();
            //   var idCurrentUser = _userManager.GetUserId(User);
            ApplicationUser userLogin = await _userManager.GetUserAsync(User);

            var userRoleLogins = await _userManager.GetRolesAsync(userLogin);

            var userRoleLoginArray = userRoleLogins.ToArray();

            var menuRoles = _menuRoleService.GetAll().ToList();

            List<MenuRole> listMenu = new List<MenuRole>();


            foreach (var menuRole in menuRoles)
            {
                if(menuRole.RoleName==null)
                    listMenu.Add(menuRole);
                foreach (var userRoleLogin in userRoleLoginArray)
                {
                    if (userRoleLogin == menuRole.RoleName)
                    {
                        listMenu.Add(menuRole);
                        break;
                    }
                }
            }

            List<MenuRoleJson.Rootobject> listrootobject = new List<MenuRoleJson.Rootobject>();
            List<MenuRoleJson.Child1> listchild1 = new List<MenuRoleJson.Child1>();
            listMenu1 =listMenu.FindAll(x=>x.MenuLevel==1);
            listMenu2 = listMenu.FindAll(x => x.MenuLevel == 2);
            listMenu3 = listMenu.FindAll(x => x.MenuLevel == 3);
            foreach (var list in listMenu2)
            {
                MenuRoleJson.Rootobject rootobject = new MenuRoleJson.Rootobject();
                rootobject.path = listMenu1.ToList().ElementAt(0).MenuLink;
                rootobject.children = getMenuChild(listMenu1.ToList().ElementAt(0).MenuId, list.MenuId);
                listrootobject.Add(rootobject);
            }

            //   var model = ViewModelMapper<ApplicationRoleViewModel, IdentityRole>.MapObjects(result.ToList(), null);
            return listrootobject;
        }

        [HttpGet]
        public async Task<List<MenuRoleViewModel>> Get()
        {
            ApplicationUser userLogin = await _userManager.GetUserAsync(User);

            var userRoleLogins = await _userManager.GetRolesAsync(userLogin);

            var userRoleLoginArray = userRoleLogins.ToArray();

            var menuRoles = _menuRoleService.GetAll().ToList();

            List<MenuRole> listMenu = new List<MenuRole>();


            foreach (var menuRole in menuRoles)
            {
          
                if (menuRole.RoleName == null)
                    listMenu.Add(menuRole);
                foreach (var userRoleLogin in userRoleLoginArray)
                {
                    if (userRoleLogin == menuRole.RoleName)
                    {
                        listMenu.Add(menuRole);
                        break;
                    }
                }
            }

            var listMenuViewModel = Mapper.Map<List<MenuRole>, List< MenuRoleViewModel> >(listMenu);
            return listMenuViewModel;
        }

        private List<MenuRoleJson.Child> getMenuChild(int parrentId, int menuId)
        {
            List<MenuRoleJson.Child> listchilds = new List<MenuRoleJson.Child>();

            foreach (var menu2 in listMenu2.FindAll(x=>x.MenuParent== parrentId &&x.MenuId== menuId))
            {  
                MenuRoleJson.Child child = new MenuRoleJson.Child();

                MenuRoleJson.Data data = new MenuRoleJson.Data();
                MenuRoleJson.Menu menu = new MenuRoleJson.Menu();
                child.path = menu2.MenuLink;
                data = new MenuRoleJson.Data();
                menu.title = menu2.MenuName;
                menu.icon = menu2.Icon;
                menu.selected = false;
                menu.order = 500;
                data.menu = menu;
                child.data = data;
                //    listchilds = getMenuChild(listMenu1.ToList().ElementAt(0).MenuId);

                child.children = getMenuChild1(menu2.MenuId);
                //   child.children = listchild1;
                listchilds.Clear();
               listchilds.Add(child);
            }
            return listchilds;
        }

        private List<MenuRoleJson.Child1> getMenuChild1(int parrentId)
        {
            List<MenuRoleJson.Child1> listchild1 = new List<MenuRoleJson.Child1>();

            var listMenu3ByParrent = listMenu3.FindAll(x => x.MenuParent == parrentId).ToList();

            foreach (var menu3 in listMenu3ByParrent)
            {
                MenuRoleJson.Child1 child1 = new MenuRoleJson.Child1();
                MenuRoleJson.Data1 data1 = new MenuRoleJson.Data1();
                MenuRoleJson.Menu1 menu1 = new MenuRoleJson.Menu1();
                menu1.title = menu3.MenuName;
                data1.menu = menu1;
                child1.path = menu3.MenuLink;
                child1.data = data1;
                listchild1.Add(child1);
            }
            return listchild1;
        }




        [HttpPost]
        public IActionResult Create([FromBody]MenuRoleViewModel menuRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IActionResult result = new ObjectResult(false);
            GenericResult addResult = null;
            try
            {

                if (menuRoleViewModel.MenuLink == "" || menuRoleViewModel.MenuLink == "/")
                    menuRoleViewModel.MenuLink = "";

                var menuTrangChu = _menuRoleService.GetMenuRoleByName("Home");


                int IdMenuCap1 = menuTrangChu.MenuId;

                
                    

                MenuRole menuRole = new MenuRole();


                menuRole = Mapper.Map<MenuRoleViewModel, MenuRole>(menuRoleViewModel);

                if (menuRole.MenuId == IdMenuCap1)
                    menuRole.MenuLevel = 1;

                if (menuRole.MenuParent == IdMenuCap1)
                    menuRole.MenuLevel = 2;
                else
                    menuRole.MenuLevel = 3;
                menuRole.MenuId = 0;
                _menuRoleService.Add(menuRole);
                _menuRoleService.Save();
                addResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Thêm thành công"
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
        public IActionResult Put([FromBody]MenuRoleViewModel menuRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IActionResult result = new ObjectResult(false);
            GenericResult updateResult = null;

            try
            {
                var menuRole = _menuRoleService.GetById(menuRoleViewModel.MenuId);
                menuRole.UpdateMenuRole(menuRoleViewModel);
             //   MenuRole menuRole = Mapper.Map<MenuRoleViewModel, MenuRole>(menuRoleViewModel);
                _menuRoleService.Update(menuRole);
                _menuRoleService.Save();
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


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _removeResult = null;

            try
            {
                _menuRoleService.Delete(id);
                _menuRoleService.Save();

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
