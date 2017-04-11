using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Ips
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Ip { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
