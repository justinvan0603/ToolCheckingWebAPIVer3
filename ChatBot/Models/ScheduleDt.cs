using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class ScheduleDt
    {
        public int Id { get; set; }
        public DateTime? SchDate { get; set; }
        public string SchTerm { get; set; }
        public string DomainId { get; set; }
        public string LinkId { get; set; }
        public DateTime? ExeDate { get; set; }
        public string Executed { get; set; }
        public string ResType { get; set; }
    }
}
