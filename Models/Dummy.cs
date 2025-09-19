using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //untuk yang Required

namespace TheMealDBApp.Models
{
    public class Dummy
    {
        [Required]
        public int nama_user { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }
        public int status { get; set; }
    }
}