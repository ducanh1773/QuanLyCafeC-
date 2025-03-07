using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{

    public class Fund
    {
        [Key]
        public int Id { get; set; }
        
        
        [Required]
        public decimal SumPrice{ get; set; }
        
        [Required]
        public string detail_status{ get; set; }
        
        public DateTime creat_at{ get; set; }
        
        public string FundName{ get; set; }
        
        
    }
    
}