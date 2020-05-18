using LoraPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoraPlatform.ViewModel
{
    public class DeviceManageViewModel
    {
        public List<Device> DataList { get; set; }

        public List<Elder> Wearers { get; set; }

        public Device device { get; set; }

        [DisplayName("配戴者")]
        [Required(ErrorMessage = "配戴者不可為空")]
        public string Wearer { get; set; }
        [DisplayName("身分證")]
        [Required(ErrorMessage = "身分證不可為空")]
        [RegularExpression("^[A-Z]{1}[1-2]{1}[0-9]{8}$", ErrorMessage = "身分證格式錯誤")]
        public string IdCard { get; set; }
    }
}