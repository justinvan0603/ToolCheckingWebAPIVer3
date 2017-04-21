﻿using System;
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
            appUser.PARENT_ID = appUserViewModel.PARENT_ID;
            appUser.DESCRIPTION = appUserViewModel.DESCRIPTION;
            appUser.RECORD_STATUS = appUserViewModel.RECORD_STATUS;
            appUser.AUTH_STATUS = appUserViewModel.AUTH_STATUS;
            appUser.CREATE_DT = appUserViewModel.CREATE_DT;
            appUser.APPROVE_DT = appUserViewModel.APPROVE_DT;
            appUser.EDIT_DT = appUserViewModel.EDIT_DT;
            appUser.MAKER_ID = appUserViewModel.MAKER_ID;
            appUser.CHECKER_ID = appUserViewModel.CHECKER_ID;
            appUser.EDITOR_ID = appUserViewModel.EDITOR_ID;
            appUser.APPTOKEN = appUserViewModel.APPTOKEN;
        }
    }
}