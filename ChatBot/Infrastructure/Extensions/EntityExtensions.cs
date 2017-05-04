using System;
using ChatBot.Data.Migrations;
using ChatBot.Model.Models;
using ChatBot.ViewModels;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdateApplicationRole(this IdentityRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            //   appRole.Description = appRoleViewModel.Description;
        }
        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {

            appUser.Id = appUserViewModel.Id;
            appUser.UserName = appUserViewModel.UserName;
            appUser.Email = appUserViewModel.Email;
            appUser.FULLNAME = appUserViewModel.FULLNAME;
            appUser.PASSWORD = appUserViewModel.PASSWORD;
            appUser.PHONE = appUserViewModel.PHONE;
            appUser.PARENT_ID = null;
            appUser.DESCRIPTION = appUserViewModel.DESCRIPTION;
            appUser.RECORD_STATUS = "1";
            appUser.AUTH_STATUS = "U";
            appUser.CREATE_DT = appUserViewModel.CREATE_DT;
            appUser.APPROVE_DT = null;
            appUser.EDIT_DT = appUserViewModel.EDIT_DT;
            appUser.MAKER_ID = appUserViewModel.MAKER_ID;
            appUser.CHECKER_ID = appUserViewModel.CHECKER_ID;
            appUser.EDITOR_ID = appUserViewModel.EDITOR_ID;
            appUser.APPTOKEN = appUserViewModel.APPTOKEN;

        }

        public static void UpdateMenuRole(this MenuRole menuRole, MenuRoleViewModel menuRoleViewModel)
        {

            menuRole.MenuId = menuRoleViewModel.MenuId;
       //     menuRole.ApplicationRole = menuRoleViewModel.ApplicationRole;
            menuRole.MenuName = menuRoleViewModel.MenuName;
            menuRole.MenuNameEl = menuRoleViewModel.MenuNameEl;
            menuRole.MenuParent = menuRoleViewModel.MenuParent;
            menuRole.MenuLink = menuRoleViewModel.MenuLink;
            menuRole.RoleName = menuRoleViewModel.RoleName;
            menuRole.Icon = menuRoleViewModel.Icon;

        }
    }
}