using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace TheMealDBApp.Models
{
    public class OrdersDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int IdCategory { get; set; }
        public string? StrCategory { get; set; }
        public int? Jml { get; set; }
        public Orders Order { get; set; }
    }
}