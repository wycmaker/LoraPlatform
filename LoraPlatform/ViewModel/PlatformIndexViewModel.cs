using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoraPlatform.Models;

namespace LoraPlatform.ViewModel
{
    public class PlatformIndexViewModel
    {
        public List<ConnectTable> ConnectDevice { get; set; }

        public List<Elder> Wears { get; set; }

        [DisplayName("裝置編號")]
        [Required(ErrorMessage = "裝置編號不可為空")]
        [RegularExpression("^[a-z0-9]{8}-([a-z0-9]{4}-){3}[a-z0-9]{12}$", ErrorMessage = "裝置編號格式錯誤")]
        [Remote("DeviceCheck", "Platform", ErrorMessage = "裝置編號不存在")]
        public string DeviceId { get; set; }
        [DisplayName("配戴者姓名")]
        [Required(ErrorMessage = "配戴者姓名不可為空")]
        [RegularExpression("^[\u4e00-\u9fa5a-zA-Z0-9]+$", ErrorMessage = "姓名格式錯誤")]
        public string Name { get; set; }
        [DisplayName("身分證")]
        [Required(ErrorMessage = "身分證不可為空")]
        [RegularExpression("^[A-Z]{1}[1-2]{1}[0-9]{8}$", ErrorMessage = "身分證格式錯誤")]
        [Remote("ElderCheck", "Platform", AdditionalFields = "Name", ErrorMessage = "配戴者資料錯誤")]
        public string IdCard { get; set; }
    }
}