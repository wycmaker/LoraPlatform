//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoraPlatform.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Members
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "帳號不可為空")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "帳號須介於8-50字元")]
        [RegularExpression("[0-9a-zA-Z_]+", ErrorMessage = "帳號只能使用數字、英文和下劃線")]
        [Remote("AccountCheck", "Home", ErrorMessage = "帳號已經存在")]
        public string Account { get; set; }
        public string Password { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email不可為空")]
        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string AuthCode { get; set; }
        public bool isAdmin { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "姓名不可為空")]
        [RegularExpression("^[\u4e00-\u9fa5a-zA-Z0-9]+$", ErrorMessage = "姓名不可接受特殊字元")]
        public string Name { get; set; }
    }
}
