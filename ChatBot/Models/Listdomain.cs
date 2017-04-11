using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Listdomain
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
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
