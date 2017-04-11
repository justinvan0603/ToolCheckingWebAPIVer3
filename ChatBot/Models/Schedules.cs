using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Schedules
    {
        public int Id { get; set; }
        public DateTime? SchDate { get; set; }
        public string SchTerm { get; set; }
        public decimal? TotalDomain { get; set; }
        public decimal? TotalLink { get; set; }
        public string IsLeaf { get; set; }
        public int? ParentId { get; set; }
        public string Notes { get; set; }
        public string ProcessStatus { get; set; }
        public DateTime? CreateDt { get; set; }
        public string MakerId { get; set; }
        public decimal? PDone { get; set; }
        public decimal? PWarning { get; set; }
        public decimal? PAlert { get; set; }
        public TimeSpan? EventTime { get; set; }
        public DateTime? ExecDate { get; set; }
    }
}
