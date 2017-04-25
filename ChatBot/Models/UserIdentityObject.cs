using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class UserIdentityObject
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FULLNAME { get; set; }
     //   public DateTime? APPROVE_DT { get; set; }
     //   public string APPTOKEN { get; set; }

        public DateTime? CREATE_DT { get; set; }
    //    public string DESCRIPTION { get; set; }
     //   public DateTime? EDIT_DT { get; set; }
        public string Email { get; set; }

       
//
   //     public string PARENT_ID { get; set; }
       // public string PASSWORD { get; set; }
    
        public int? PHONE { get; set; }

        
       // public string AUTH_STATUS { get; set; }
       // public string CHECKER_ID { get; set; }
       // public string EDITOR_ID { get; set; }
      //  public string MAKER_ID { get; set; }
       // public string RECORD_STATUS { get; set; }
    }
}
