using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Userconfig
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ConfType { get; set; }
        public string ConfValue { get; set; }
    }
}
