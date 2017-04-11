using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class UserDomainSearchObject
    {
        public int ID { get; set; }
        public string USER_ID { get; set; }
        public string DOMAIN_ID { get; set; }
        public string NOTES { get; set; }
        public string FULLNAME { get; set; }
        public int? USERID { get; set; }
        public int? PARENT_ID { get; set; }
    }
}
