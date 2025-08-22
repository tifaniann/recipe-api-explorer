using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheMealDBApp.DTOs
{
    public class Categories_Temp
    {
        public string? IdCust { get; set; }
        public string? IdCategory { get; set; }
        public string? StrCategory { get; set; }
        public int? Jml { get; set; }
    }
}