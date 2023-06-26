using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElphatehApp.Models
{
    public class Registration
    {
        [Display(Name ="اسم المستخدم")][MinLength(5,ErrorMessage ="يجب ان يذيد الاسم عن 5 احرف")]
        public String UserName { get; set; }
        
        [Display(Name = "الرقم السري")]
        [MinLength(5, ErrorMessage = "يجب ان يذيد الرقم السري عن 6 احرف")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DataType(DataType.Password)][Compare("Password",ErrorMessage ="غير مطابق للرقم السري")]
        [Display(Name = "التاكيد")]
        public String ConfirmPassword { set; get; }
    }
}
