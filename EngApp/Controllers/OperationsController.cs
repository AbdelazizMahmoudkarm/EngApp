using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EngApp.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElphatehApp.Models
{
    [Authorize(Roles = "Admin")]
    public class OperationsController : Controller
    {
        public readonly AppDb _context;
        public OperationsController(AppDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CalcBilsWithDate(string date,string date2)
        {
            var custrans = _context.CustTrans.FromSqlRaw("Select * from dbo.CustTrans where CustTrans.date > {0} or date < {1}", date,date2).Where(x=>x.isdel==false).Include(x=>x.Customer).Include(x=>x.Transaction).ToList();
            return PartialView(custrans);
        }
        public IActionResult day()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var paid = _context.CustTrans.Where(x => x.date.Contains(date)&&x.isdel==false).Sum(x => x.paid);
            return PartialView(paid);
        }
        public IActionResult Deletecust()
        {
            backup();
            var custrans = _context.CustTrans.ToList();
            foreach(var item in custrans)
            {
                if(item.totalmoney==item.paid&&item.change==0)
                {
                    _context.RemoveRange(_context.paperDatas.Where(x => x.custtransId == item.custtransId));
                    if (_context.Customers.Where(x => x.customerId == item.customerId).Count() == 1)
                        _context.Remove(_context.Customers.Where(x => x.customerId == item.customerId).FirstOrDefault());
                    _context.SaveChanges();
                }
            }
            return Ok();
        }
        public IActionResult Endedopration()
        {
            var custrans = _context.CustTrans.Where(x => x.end == true).Include(x=>x.Customer).Include(x=>x.Transaction).ToList();
            return PartialView(custrans);
        }
        public void backup()
        {
            if (!System.IO.Directory.Exists(@"D:\data"))
                System.IO.Directory.CreateDirectory(@"D:\data");
            if (!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString());
            if (!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString());
            if (!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString());
            string filename = String.Format("{0}__{1}", customedate(), ".bak" + "'");
            _context.Database.ExecuteSqlCommand("BACKUP DATABASE Eng TO  DISK ='D://data/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + filename);
        }
        public String customedate()
        {
            var hour = DateTime.Now.Hour;
            var datenow = DateTime.Now;
            String date = null;
            if (hour > 12)
                date = (hour - 12) + "--" + datenow.Minute + "-" + datenow.Second + " م";
            else if (hour == 12)
                date = hour + "--" + datenow.Minute + "-" + datenow.Second + " م";
            else
                date = hour + "--" + datenow.Minute + "-" + datenow.Second + " ص";
            return date;
        }
    }
}