using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class UserDomain
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DomainId { get; set; }
        public string Notes { get; set; }
    }
}
