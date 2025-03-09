using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{

    public class OrderDetailProduct
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("ProductCoffee")]
        public int Id_Product{get;set;}
        
        [ForeignKey("Order")]
        public int Id_Order{get;set;}
        
        public int Quantity { get; set; }
        
        public virtual ProductCoffee ProductCoffee{ get; set; }
        
        public virtual OrderCoffe OrderCoffe{ get; set; }
        
        
    }
}
