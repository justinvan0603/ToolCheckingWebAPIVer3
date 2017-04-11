using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Features
    {
        public int Id { get; set; }
        public string FeaType { get; set; }
        public string Contents { get; set; }
        public int? Level { get; set; }
        public string Resource { get; set; }
        public string RecordStatus { get; set; }
        public string AuthStatus { get; set; }
        public DateTime? ApproveDt { get; set; }
        public DateTime? EditDt { get; set; }
        public string CheckerId { get; set; }
        public string EditorId { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
    }
}
