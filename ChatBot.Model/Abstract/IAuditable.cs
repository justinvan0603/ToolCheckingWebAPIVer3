using System;

namespace ChatBot.Model.Abstract
{
    public interface IAuditable
    {
        DateTime? CreatedDate { set; get; }
        string CreatedBy { set; get; }
        DateTime? UpdatedDate { set; get; }
        string UpdatedBy { set; get; }
        bool Status { set; get; }


        //bool RECORD_STATUS { set; get; }
        //DateTime? RECORD_STATUS { set; get; }
        //bool AUTH_STATUS { set; get; }
        //DateTime? APPROVE_DT { set; get; }
        //DateTime? EDIT_DT { set; get; }





    }
}