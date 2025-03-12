using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
namespace QuanLyCafe.Models
{

    public class Supply
    {
        [Key]
        public int id { get; set; }
        
        [ForeignKey("Account")]
        public int Id_Account { get; set; }
        [Required]
        public DateTime Time_In { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Deleted { get; set; }
        public ICollection<DetailSupplyStock> DetailSupplyStocks { get; set; }
        
        
        [JsonIgnore]
        public virtual Account Account { get; set; }
        
        
        [JsonIgnore]
        
        public ICollection<PaymentForm> paymentForms{ get; set; }   
        
        
        
        
        
    }
}