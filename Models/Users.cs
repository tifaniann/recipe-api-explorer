using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  //untuk yang Required
using System.Linq;
using System.Threading.Tasks;

namespace TheMealDBApp.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)] // panjang maksimal username
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string Password { get; set; } = string.Empty;
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
    }
}