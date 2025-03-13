using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
namespace QuanLyCafe.Models
{
    public class ProductCoffee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Detail { get; set; }

        public decimal price { get; set; }

        public string category_Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Status { get; set; }

        public bool Deleted { get; set; }

        public string ImageProduct { get; set; }

        [JsonIgnore]
        public ICollection<DeatailStockProduct> deatailStockProducts { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetailProduct> OrderDetailProducts { get; set; }


    }
}
