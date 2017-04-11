using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Models
{
    public class UserConfigObject
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string CONF_TYPE { get; set; }
        public string CONF_VALUE { get; set; }
        public string CONF_NAME { get; set; }
        public string NOTES { get; set; }
        public string VALUE_TYPE { get; set; }
    }
}
