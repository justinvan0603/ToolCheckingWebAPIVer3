using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChatBot.Data;
using ChatBot.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.Infrastructure
{
    public static class DbInitializer
    {
        private static ChatBotDbContext _context;

        public static void Initialize(IServiceProvider serviceProvider, string imagesPath)
        {
            _context = (ChatBotDbContext)serviceProvider.GetService(typeof(ChatBotDbContext));
            InitializeUserRoles();
        }

        private static void InitializeUserRoles()
        {
          //  var manager = new UserManager<ApplicationUser>(new ChatBotDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ChatBotDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "admin",
            //    Email = "admin@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    FullName = "Technology Education"

            //};

            //manager.Create(user, "12345678");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "Manager" });
            //    roleManager.Create(new IdentityRole { Name = "GiangVien" });
            //    roleManager.Create(new IdentityRole { Name = "HocVien" });
            //}

            //  var adminUser = manager.FindByEmail("admin@gmail.com");

            //  manager.AddToRoles(adminUser.Id, new string[] { "Admin", "Manager" });
        }
    }
}