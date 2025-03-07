using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{

    public class OrderCoffe
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Id_Account{ get; set; }

        
        public DateTime TimeOrder{get; set; }
        
        public bool Status { get; set; }
        
        public bool Deleted { get; set; }
        
    }

}