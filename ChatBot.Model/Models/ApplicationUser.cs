using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Password { get; set; }
       // public string Email { get; set; }
        public int? Phone { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string RecordStatus { get; set; }
        public string AuthStatus { get; set; }
        public DateTime? CreateDt { get; set; }
        public DateTime? ApproveDt { get; set; }
        public DateTime? EditDt { get; set; }
        public string MakerId { get; set; }
        public string CheckerId { get; set; }
        public string EditorId { get; set; }
        public string Apptoken { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
        public static implicit operator ApplicationUser(IdentityRole v)
        {
            throw new NotImplementedException();
        }
    }
}