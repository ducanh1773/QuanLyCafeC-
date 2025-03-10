using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{

    public class TableCoffe
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        
        public string TableName { get; set; }
        
        [Required]
        
        public int ChairNumber{ get; set; }
        
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Deleted { get; set; }
        
        
       
        
        
    }



}