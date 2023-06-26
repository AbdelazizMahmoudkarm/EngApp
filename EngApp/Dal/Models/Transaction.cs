using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class Transaction
    {
        public int transactionId { get; set; }
        [Display(Name="المعامله")]
        public String transactionName { get; set; }
        public bool isdel { get; set; }
    }
}
