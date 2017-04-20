using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Model.Models
{
    public class MenuRole
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { set; get; }

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
