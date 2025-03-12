using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuanLyCafe.Models
{
    public class DetailSupplyStock
    {
        [Key] // Đánh dấu thuộc tính này là khóa chính
        public int Id { get; set; }
        
        [ForeignKey("Supply")] // Foreign key reference to Supply
        public int ID_Supply { get; set; }

        [ForeignKey("Stock")] // Foreign key reference to Stock
        public int Id_Stock { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual Supply Supply { get; set; }
        public virtual Stock Stock { get; set; }
    }
}