using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngApp.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeletedActions : Controller
    {
        private readonly AppDb _context;
     
        public DeletedActions(AppDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var custrans = _context.CustTrans.Where(x=>x.isdel==true).Include(x => x.Customer).Include(x => x.Transaction).ToList();
            return View(custrans);
        }
        public IActionResult Search(String name)
        {
            var custransdata = _context.CustTrans.Where(x => x.isdel == true).Include(x => x.Customer).Where(x => x.Customer.customerName.Contains(name)).Include(x => x.Transaction).ToList();
            return Ok(custransdata);
        }
    }
}
