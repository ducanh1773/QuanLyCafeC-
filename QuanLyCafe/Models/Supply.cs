using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{

    public class Supply
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int Id_Account { get; set; }
        [Required]
        public DateTime Time_In { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Deleted { get; set; }
        public ICollection<DetailSupplyStock> DetailSupplyStocks { get; set; }
    }
}