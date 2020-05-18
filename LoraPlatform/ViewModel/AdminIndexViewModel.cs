using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoraPlatform.Models;

namespace LoraPlatform.ViewModel
{
    public class AdminIndexViewModel
    {
        public List<Device> ConnectDevice { get; set; }

        public List<Elder> Wears { get; set; }

        public string Device { get; set; }
    }
}