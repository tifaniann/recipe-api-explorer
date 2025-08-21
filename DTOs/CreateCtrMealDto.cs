using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheMealDBApp.DTOs
{
    [Table("Category_temp")]
    public class CreateCtrMealDto
    {
        [Key]
        public string? IdCategory { get; set; }
        public string? StrCategory { get; set; }

    }
}