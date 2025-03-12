using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
namespace QuanLyCafe.Models
{
    public class Account
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required , EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        [Required ]
        public string Password { get; set; }
        
        
        [Required]
        public DateTime Create_At { get; set; }
        
        [Required]
        public bool Status { get; set; }
        
        
        [Required]
        public bool Deleted { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<Supply> Supplies { get; set; }
        
    }
}