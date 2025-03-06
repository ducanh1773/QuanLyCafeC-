using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{

    public class PaymentForm
    {
        [Key]
        public int Id { get; set; }
        
        public int Id_Order{ get; set; }
        public int ID_Supply{ get; set; }
        
        [Required]
        public int Id_Fund{ get; set; }
        
        [Required]
        public string Payment_Method{get; set; }
        
        public DateTime CreatedAt{ get; set; }
        
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Deleted { get; set; }
        public decimal Sum_Price{get; set; }
    }

}