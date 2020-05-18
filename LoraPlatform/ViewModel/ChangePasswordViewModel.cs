using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoraPlatform.ViewModel
{
    public class ChangePasswordViewModel
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "舊密碼不可為空")]
        public string OldPassword { get; set; }

        [DisplayName("新密碼")]
        [Required(ErrorMessage = "新密碼不可為空")]
        [RegularExpression("[A-Za-z0-9_]+", ErrorMessage = "密碼只接受英文字母、數字和下劃線")]
        [MinLength(6, ErrorMessage = "密碼長度須大於6字元")]
        public string NewPassword { get; set; }

        [DisplayName("新密碼確認")]
        [Required(ErrorMessage = "新密碼確認不可為空")]
        [Compare("NewPassword", ErrorMessage = "兩次密碼不一致")]
        public string ComfirmNewPassword { get; set; }

        [DisplayName("驗證碼")]
        [Required(ErrorMessage = "驗證碼不可為空")]
        public string ValidateCode { get; set; }
    }
}