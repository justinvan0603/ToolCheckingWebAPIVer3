using System;
using ChatBot.Model.Models;
using ChatBot.ViewModels;
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
            //appUser.Id = appUserViewModel.Id;
            //appUser.Email = appUserViewModel.Email;
            //appUser.UserName = appUserViewModel.UserName;
            //appUser.PhoneNumber = appUserViewModel.PhoneNumber;
        }
    }
}