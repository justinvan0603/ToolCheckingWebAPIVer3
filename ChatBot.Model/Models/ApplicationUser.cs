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
        // ID 
        //USERNAME
        [StringLength(200)]
        public string FULLNAME { get; set; }
        public string PASSWORD { get; set; } //varchar(50) -> max
        // public string Email { get; set; }

        public int? PHONE { get; set; }

        [StringLength(450)]
        public string PARENT_ID { get; set; }
        [StringLength(500)]
        public string DESCRIPTION { get; set; }
        [StringLength(1)]
        public string RECORD_STATUS { get; set; }
        [StringLength(1)]
        public string AUTH_STATUS { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public DateTime? EDIT_DT { get; set; }
        [StringLength(15)]
        public string MAKER_ID { get; set; }
        [StringLength(15)]
        public string CHECKER_ID { get; set; }
        [StringLength(15)]
        public string EDITOR_ID { get; set; }
        [StringLength(1000)]
        public string APPTOKEN { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
  
    }
}