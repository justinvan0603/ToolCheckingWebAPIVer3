using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuNameEl { get; set; }
        public string MenuParent { get; set; }
        public string MenuLink { get; set; }
        public int? MenuOrder { get; set; }
        public string AuthStatus { get; set; }
        public string MakerId { get; set; }
        public string CheckerId { get; set; }
        public DateTime? DateApprove { get; set; }
        public string Isapprove { get; set; }
        public string IsapproveFunc { get; set; }
    }
}
