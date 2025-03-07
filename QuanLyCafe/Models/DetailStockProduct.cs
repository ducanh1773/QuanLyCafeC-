using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{
    public class DeatailStockProduct
    {
        [Key]
        public int Id { get; set; }
        
        public int Id_Product{ get; set; }
        
        public int Id_StockProduct{ get; set; } 
        
        public string Ingredient{ get; set; }
        
        public int Quantity { get; set; }
        
        
    }

}