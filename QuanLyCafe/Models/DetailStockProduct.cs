using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{
    public class DeatailStockProduct
    {
        [Key]
        public int Id { get; set; }
        
        
        [ForeignKey("ProductCoffee")]
        public int Id_Product{ get; set; }
        
        
        [ForeignKey("Stock")]
        public int Id_StockProduct{ get; set; } 
        
        public int Quantity { get; set; }
        
        public virtual Stock Stock { get; set; }
        
        public virtual ProductCoffee ProductCoffee{ get; set; } 
        
        
        
    }

}