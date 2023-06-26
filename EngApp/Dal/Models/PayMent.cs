using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class PayMent
    {
        public int paymentId { get; set; }
        [ForeignKey("CustTrans")]
        public int custtransId { get; set; }
        public CustTrans CustTrans { set; get; }
        [Display(Name = "المدفوع")]
        public Double paid { get; set; }
        [Display(Name = "التاريخ")]
        public String date { get; set; }
        public bool isdel  { get; set; }
    }
}
