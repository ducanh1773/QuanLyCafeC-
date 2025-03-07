using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{

    public class DetailSupplyStock
    {
        [Key] // Đánh dấu thuộc tính này là khóa chính
        public int Id { get; set; }
        
        public int ID_Supply { get; set; }

        public int Id_Stock { get; set; }

        [Required]
        public int Quantity { get; set; }

      
        

    }

}