using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{

    public class OrderDetailProduct
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int Id_Product{get;set;}
        
        [Required]
        public int Id_Order{get;set;}
        
        public int Quantity { get; set; }
        
        
    }
}
