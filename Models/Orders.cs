using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace TheMealDBApp.Models
{
    public class Orders
    {
        public int OrderID { get; set; }
        public int IDcust { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
    }
}