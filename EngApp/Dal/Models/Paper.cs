using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EngApp.Dal.Models
{
    public class Paper
    {
        public int paperId { get; set; }
        [Display(Name = "الورق")]
        public String paperName { get; set; }
        public bool  isdel { get; set; }
    }
}
