using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class PaperData
    {
        public int paperdataId { get; set; }
        [ForeignKey("Paper")]
        public int paperId { get; set; }
        public Paper Paper { get; set; }
        [ForeignKey("CustTrans")]
        public int custtransId { get; set; }
        public CustTrans CustTrans { set; get; }
        [Display(Name="الورق")]
        public String paperName { get; set; }
        [Display(Name ="مطلوب")]
        public bool required { get; set; }
        public bool isdel { get; set; }
    }
}
