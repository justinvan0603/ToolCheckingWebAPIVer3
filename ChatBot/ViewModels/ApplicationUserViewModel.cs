using System;
using System.Collections.Generic;
using ChatBot.Model.Models;

namespace ChatBot.ViewModels
{
    public class ApplicationUserViewModel: BaseViewModel<ApplicationUser>
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
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
        public string Domain { get; set; }
        public string DomainDesc { get; set; }
        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }
    }
}