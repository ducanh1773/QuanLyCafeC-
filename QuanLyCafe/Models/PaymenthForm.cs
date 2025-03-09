using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCafe.Models
{
    public class PaymentForm
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("OrderCoffe")]
        
        public int Id_Order { get; set; }
        
        [ForeignKey("Supply")] // Foreign key reference to Supply
        public int ID_Supply { get; set; }
        
        [ForeignKey("Fund")]
        public int Id_Fund { get; set; }
        
        [Required]
        public string Payment_Method { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public bool Status { get; set; }
        
        [Required]
        public bool Deleted { get; set; }
        
        public decimal Sum_Price { get; set; }

        // Navigation property
        public virtual Supply Supply { get; set; }
        
        public virtual Fund Fund{get; set;}
        
        public virtual OrderCoffe OrderCoffe{get; set;}
        
        
        
        
    }
}