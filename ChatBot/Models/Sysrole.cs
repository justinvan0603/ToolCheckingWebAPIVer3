using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Sysrole
    {
        public string RoleId { get; set; }
        public string RoleDesc { get; set; }
        public string MakerId { get; set; }
        public string AuthStatus { get; set; }
        public string CheckerId { get; set; }
        public DateTime? DateApprove { get; set; }
        public string Isapprove { get; set; }
    }
}
