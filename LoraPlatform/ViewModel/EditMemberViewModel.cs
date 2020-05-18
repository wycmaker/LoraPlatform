using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoraPlatform.ViewModel
{
    public class EditMemberViewModel
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "帳號不可為空")]
        public string account { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "姓名不可為空")]
        [RegularExpression("^[\u4e00-\u9fa5a-zA-Z0-9]+$", ErrorMessage = "姓名不可接受特殊字元")]
        public string name { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email不可為空")]
        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        public string email { get; set; }

        public string phone { get; set; }
    }
}