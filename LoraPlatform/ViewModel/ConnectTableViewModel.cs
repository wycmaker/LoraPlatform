using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoraPlatform.Models;

namespace LoraPlatform.ViewModel
{
    public class ConnectTableViewModel
    {
        public List<ConnectTable> connectTables { get; set; }

        public List<Elder> Wears { get; set; }

        public List<Members> ConnectAccount { get; set; }
    }
}