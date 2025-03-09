using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
namespace QuanLyCafe.Models
{

    public class OrderCoffe
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Account")]
        public int Id_Account{ get; set; }

        
        public DateTime TimeOrder{get; set; }
        
        public bool Status { get; set; }
        
        public bool Deleted { get; set; }
        
        public virtual Account Account { get; set; }
        
        public ICollection<OrderDetailProduct> orderDetailProducts { get; set; }
        
        public ICollection<DetailTableOrder> detailTableOrders { get; set; }    
        
        public ICollection<PaymentForm> paymentForms{ get; set; }
        
    }

}