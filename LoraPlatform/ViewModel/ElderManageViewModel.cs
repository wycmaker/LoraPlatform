using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoraPlatform.Models;

namespace LoraPlatform.ViewModel
{
    public class ElderManageViewModel
    {
        public List<Elder> DataList { get; set; }

        public Elder NewElder { get; set; }

        public string birthday { get; set; }
    }
}