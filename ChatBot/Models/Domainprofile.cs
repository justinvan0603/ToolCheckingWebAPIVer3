using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Domainprofile
    {
        public int Id { get; set; }
        public int? DomainId { get; set; }
        public string Domain { get; set; }
        public string Type { get; set; }
        public string Values { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
    }
}
