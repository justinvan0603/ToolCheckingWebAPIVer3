using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Logs
    {
        public int Id { get; set; }
        public int? LogCode { get; set; }
        public int? TrnId { get; set; }
        public string LogType { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
    }
}
