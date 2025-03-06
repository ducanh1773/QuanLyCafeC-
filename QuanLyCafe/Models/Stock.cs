using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Deleted { get; set; }
        [Required]
        public string UnitOfMeasure{get; set; }
        
        
        
        
    }
}
