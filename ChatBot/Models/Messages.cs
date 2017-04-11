using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Messages
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string User { get; set; }
        public string Domain { get; set; }
        public string Status { get; set; }
        public DateTime CreateDt { get; set; }
        public string Type { get; set; }
        public string Keyterm { get; set; }
        public string Icon { get; set; }
    }
}
