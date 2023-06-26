using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class CustTrans
    {
      [Display(Name="رقم العمليه")]
        public int custtransId { get; set; }
        [ForeignKey("Customer")]
        public int customerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Transaction")]
        public int transactionId { get; set; }
        public Transaction Transaction { set; get; }
        [Display(Name = "التاريخ")]
        public String date { get; set; }
        [Display(Name = "المبلغ الكلي")]
        public double totalmoney { get; set; }
        [Display(Name = "المدفوع")]
        public double paid { get; set; }
        [Display(Name = "الباقي")]
        [NotMapped]
        public double change { get; set; }
        public bool end { get; set; }
        public string enddate { get; set; }
        public List<PaperData> paperlist { set; get; }
        public List<PayMent> paymentlist { set; get; }
        public bool isdel { get; set; }
    }
}
