using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Options
    {
        public int Id { get; set; }
        public string DomainId { get; set; }
        public string IsLimit { get; set; }
        public int? Times { get; set; }
        public string Description { get; set; }
        public string RecordStatus { get; set; }
        public string AuthStatus { get; set; }
        public DateTime? CreateDt { get; set; }
        public DateTime? ApproveDt { get; set; }
        public DateTime? EditDt { get; set; }
        public string MakerId { get; set; }
        public string CheckerId { get; set; }
        public string EditorId { get; set; }
    }
}
