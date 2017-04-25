using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.ViewModels.AccountViewModels
{
    public class ChangePasswordViewModel { 
    
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu mới ít nhất {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        ////[Display(Name = "Nhập lại mật khẩu")]
        //[Compare("NewPassword", ErrorMessage = "Mật khẩu củ và mật khẩu mới không trùng khớp")]
        //public string ConfirmPassword { get; set; }
    }
}
