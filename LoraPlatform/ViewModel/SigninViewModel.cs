using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoraPlatform.ViewModel
{
    public class SigninViewModel
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "帳號不可為空")]
        public string Account { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "密碼不可為空")]
        public string Password { get; set; }
    }
}