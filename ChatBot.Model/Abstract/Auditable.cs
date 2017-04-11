using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatBot.Model.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }


        [DefaultValue(true)]
        public bool Status
        {
            set;
            get;
        }
    }
}