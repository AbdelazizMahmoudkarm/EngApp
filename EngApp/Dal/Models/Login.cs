using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElphatehApp.Models
{
    public class Login
    {
        [Display(Name ="اسم المستخدم")][Required]
        public String UserName { get; set; }
        [Display(Name ="الرقم السري")]
        [DataType(DataType.Password)][Required]
        public String Password { get; set; }
    }
}
