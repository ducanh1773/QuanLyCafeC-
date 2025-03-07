using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{
    public class DetailTableOrder
    {
        [Key]
        public int id{ get; set; }
        [Required]
        public int Id_Table{ get; set; }
        [Required]
        public int Id_Order{ get; set; }
        
    }

}