using System;
using System.Collections.Generic;

namespace ChatBot.Models
{
    public partial class Configtype
    {
        public int Id { get; set; }
        public string ConfType { get; set; }
        public string ConfName { get; set; }
        public string ValueType { get; set; }
        public int? Prioritize { get; set; }
        public string Notes { get; set; }
    }
}
