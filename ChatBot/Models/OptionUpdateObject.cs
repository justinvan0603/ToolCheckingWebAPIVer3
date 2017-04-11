using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class OptionUpdateObject
    {
        //int ID { get; set; }
        //string DOMAIN_ID { get; set; }
        //string IS_LIMIT { get; set; }
        //int? TIMES { get; set; }
        //string DESCRIPTION { get; set; }
        //string RECORD_STATUS { get; set; }
        //string AUTH_STATUS { get; set; }
        //DateTime? CREATE_DT { get; set; }
        //DateTime? APPROVE_DT { get; set; }
        //DateTime? EDIT_DT { get; set; }
        //string MAKER_ID { get; set; }
        //string CHECKER_ID { get; set; }
        //string EDITOR_ID { get; set; }
        public OptionSearchObject OPTION { get; set; }
        public List<Optionlinks> DOMAINLINK { get; set; }
        public List<UserDomainSearchObject> DOMAINUSER { get; set; }
        public string IsEditUser { get; set; }
        public string IsEditLink { get; set; }
    }
}
