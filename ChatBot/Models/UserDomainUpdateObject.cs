using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class UserDomainUpdateObject
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DomainId { get; set; }
        public string Notes { get; set; }
        public bool IsChecked { get; set; }
    }
}
