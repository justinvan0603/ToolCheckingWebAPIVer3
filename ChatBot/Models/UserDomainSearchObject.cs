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
        public string USERID { get; set; }
        public string PARENT_ID { get; set; }
    }
}
