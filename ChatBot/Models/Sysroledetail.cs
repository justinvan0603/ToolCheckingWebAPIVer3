using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Sysroledetail
    {
        public string RoleId { get; set; }
        public string MenuId { get; set; }
        public bool Isinsert { get; set; }
        public bool Isupdate { get; set; }
        public bool Isdelete { get; set; }
        public bool Isedit { get; set; }
        public bool Issearch { get; set; }
        public bool Isview { get; set; }
        public bool Isapprove { get; set; }
        public bool Isclose { get; set; }
        public bool? Ischecknull { get; set; }
    }
}
