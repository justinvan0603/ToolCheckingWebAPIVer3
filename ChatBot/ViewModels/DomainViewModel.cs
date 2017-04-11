using System;

namespace ChatBot.ViewModels
{
    public class DomainViewModel
    {
        public int Id { get; set; }
        public int DOMAIN_ID { get; set; }
        public string DOMAIN { get; set; }
        public bool Status { get; set; }
        public DateTime? UpdatedDate { set; get; }
    }
}
