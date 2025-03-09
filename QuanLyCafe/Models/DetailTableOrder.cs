using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using QuanLyCafe.Models;
namespace QuanLyCafe.Models
{
    public class DetailTableOrder
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("TableCoffe")]
        public int Id_Table { get; set; }
        [ForeignKey("OrderCoffe")]
        public int Id_Order { get; set; }
        
        public virtual TableCoffe TableCoffe{ get; set; }
        
        public virtual OrderCoffe OrderCoffe{ get; set; }
        
        
        

    }

}