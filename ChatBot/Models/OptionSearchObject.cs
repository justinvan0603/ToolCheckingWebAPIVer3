using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class OptionSearchObject
    {
        public int ID { get; set; }
        public string DOMAIN_ID { get; set; }
        public string IS_LIMIT { get; set; }
        public int? TIMES { get; set; }
        public string DESCRIPTION { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public DateTime? EDIT_DT { get; set; }
        public string MAKER_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string EDITOR_ID { get; set; }

        public string USERNAME { get; set; }
        public string FULLNAME { get; set; }
        public string PARENT_ID { get; set; }
    }
}
