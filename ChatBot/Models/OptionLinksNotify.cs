using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class OptionLinksNotify
    {
        public int Id { get; set; }
        public int? OptionsId { get; set; }
        public string DomainId { get; set; }
        public string Link { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
        public UserDomainNotify UserDomainNotify { get; set; }
    }
}
