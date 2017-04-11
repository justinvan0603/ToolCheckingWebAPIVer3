using System;
using ChatBot.Model.Abstract;

namespace ChatBot.Model.Models
{
    public class Error : IEntityBase
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
