using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyCafe.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public DateTime creat_at { get; set; }
        public bool status { get; set; }
        public bool deleted { get; set; }
    }
}
