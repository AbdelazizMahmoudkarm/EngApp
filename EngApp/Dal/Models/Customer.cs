using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class Customer
    {
        public int customerId{set;get;}
        [Display(Name ="العميل")]
        public String customerName { set; get; }
        public bool isdel { get; set; }
    }
}
