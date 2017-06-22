using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class UserDomainNotify
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string DOMAIN_ID { get; set; }
        public string IS_TOUT { get; set; }
        public string IS_CIP { get; set; }
        public string IS_RDOM { get; set; }
        public string IS_ECODE { get; set; }
        public string IS_CCON { get; set; }
    }
}
