using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatBot.Model.Models;

namespace ChatBot.ViewModels
{
    public class MenuRoleViewModel
    {
        public int MenuId { get; set; }
     //   public virtual ApplicationRole ApplicationRole { set; get; }

        public string MenuName { get; set; }
        public string MenuNameEl { get; set; }
        public int? MenuParent { get; set; }
        public string MenuLink { get; set; }

      //  public int? MenuOrder { get; set; }
        //public string AuthStatus { get; set; }
        //public string MakerId { get; set; }
        //public string CheckerId { get; set; }
     //   public DateTime? DateApprove { get; set; }
        //public string Isapprove { get; set; }
        //public string IsapproveFunc { get; set; }

        public string RoleName { get; set; }
        public string Icon { get; set; }
 //       public string Selected { get; set; }
  //      public string Expanded { get; set; }
   //     public bool Status { get; set; }
    }
}
