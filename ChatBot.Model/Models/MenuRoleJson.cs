using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Model.Models
{
    public class MenuRoleJson
    {

        public class Rootobject
        {
            public string path { get; set; }
            public List<Child> children { get; set; }
        }

        public class Child
        {
            public string path { get; set; }
            public Data data { get; set; }
            public List<Child1> children { get; set; }
        }

        public class Data
        {
            public Menu menu { get; set; }
        }

        public class Menu
        {
            public string title { get; set; }
            public string icon { get; set; }
            public bool selected { get; set; }
            public bool expanded { get; set; }
            public int order { get; set; }
        }

        public class Child1
        {
            public string path { get; set; }
            public Data1 data { get; set; }
        }

        public class Data1
        {
            public Menu1 menu { get; set; }
        }

        public class Menu1
        {
            public string title { get; set; }
        }



    }
}
