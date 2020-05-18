using LoraPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoraPlatform.ViewModel
{
    public class SignupViewModel
    {
        public Members NewMember { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "密碼不可為空")]
        [MinLength(8, ErrorMessage = "密碼需大於8字元")]
        [RegularExpression("[0-9a-zA-Z_]+", ErrorMessage = "密碼只能使用數字、英文和下劃線")]
        public string Password { get; set; }
        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "確認密碼不可為空")]
        [MinLength(8, ErrorMessage = "密碼需大於8字元")]
        [Compare("Password", ErrorMessage = "兩次輸入密碼不相同")]
        [RegularExpression("[0-9a-zA-Z_]+", ErrorMessage = "密碼只能使用數字、英文和下劃線")]
        public string ComfirmPassword { get; set; }
    }
}