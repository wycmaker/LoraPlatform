using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoraPlatform.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [DisplayName("會員帳號")]
        [Remote("AccountExist", "Home", ErrorMessage = "此帳號不存在，請重新確認您的帳號")]
        [Required(ErrorMessage = "會員帳號不可為空")]
        public string Account { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email不可為空")]
        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        public string Email { get; set; }
    }
}