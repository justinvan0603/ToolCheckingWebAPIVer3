using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Optionlinks
    {
        public int Id { get; set; }
        public int? OptionsId { get; set; }
        public string DomainId { get; set; }
        public string Link { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
    }
}
