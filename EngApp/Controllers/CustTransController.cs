using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EngApp.Dal;
using EngApp.Dal.Models;

namespace EngApp.Controllers
{
    public class CustTransController : Controller
    {
        private readonly AppDb _context;

        public CustTransController(AppDb context)
        {
            _context = context;
        }
        [Produces("application/json")]
        public IActionResult Search(String name)
        {
            var custransdata = _context.CustTrans.Where(x=>x.isdel==false).Include(x => x.Customer).Where(x => x.Customer.customerName.Contains(name)).Include(x => x.Transaction).ToList();
            return Ok(custransdata);
        }
        // GET: CustTrans
        public async Task<IActionResult> Index(int ? pageNumber)
        {
            var appDb = _context.CustTrans.Include(c => c.Customer).Include(c => c.Transaction).Where(x=>x.isdel==false);
            return View(await PaginatedList<CustTrans>.CreateAsync(appDb,pageNumber ?? 1, 9));
        }
        // GET: CustTrans/Create
        public IActionResult Create()
        {
            return PartialView();
        }
        public IActionResult paperdata(int id)
        {
            var paper = _context.CustTrans.Where(x => x.custtransId == id).Include(x=>x.Customer).Include(x=>x.Transaction).Include(x=>x.paperlist).Include(x=>x.paymentlist).FirstOrDefault();
            return View(paper);
        }

        // POST: CustTrans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustTrans custTrans)
        {
                custTrans.date = date();
                custTrans.change = custTrans.totalmoney - custTrans.paid;
                var oldcustomer = _context.Customers.Where(x => x.customerName == custTrans.Customer.customerName).FirstOrDefault();
                if (oldcustomer != null)
                {
                    custTrans.Customer.customerId = oldcustomer.customerId;
                    custTrans.customerId = oldcustomer.customerId;
                    custTrans.Customer = null;
                }
                else
                {
                    if (_context.Customers.Count() == 0)
                    {
                        custTrans.Customer.customerId = 1;
                        custTrans.customerId = 1;
                    }
                    else
                    {
                        custTrans.Customer.customerId = _context.Customers.Max(x => x.customerId) + 1;
                        custTrans.customerId = custTrans.Customer.customerId;
                    }
                }
                if (_context.CustTrans.Count() == 0)
                    custTrans.custtransId = 1;
                else
                    custTrans.custtransId = 1 + _context.CustTrans.Max(x => x.custtransId);
                if (custTrans.paid != 0)
                {
                    if(custTrans.paymentlist ==null)
                        custTrans.paymentlist= new List<PayMent>();
                    PayMent p = new PayMent();
                    if(_context.payMents.Count()==0)
                        p.paymentId = 1;
                    else
                        p.paymentId = 1+_context.payMents.Max(x=>x.paymentId);
                    p.custtransId = custTrans.custtransId;
                    p.paid = custTrans.paid;
                    p.date = date();
                    p.isdel = false;
                    custTrans.paymentlist.Add(p);
                }
            if (custTrans.paperlist != null)
            {
                int paperdataid = 1;
                if (_context.paperDatas.Count() == 0)
                {
                    foreach (var data in custTrans.paperlist)
                    {
                        data.custtransId = custTrans.custtransId;
                        data.paperdataId = paperdataid++;
                    }
                }
                else
                {
                    foreach (var data in custTrans.paperlist)
                    {
                        data.custtransId = custTrans.custtransId;
                        data.paperdataId = _context.paperDatas.Max(x => x.paperdataId) + paperdataid++;
                    }
                }
            }
                _context.Add(custTrans);
                 await _context.SaveChangesAsync();
                 backup();
                return Ok();
        }
        // GET: CustTrans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var custTrans = await _context.CustTrans.Include(x=>x.Customer).Include(x=>x.paperlist).Include(x=>x.Transaction).Where(x=>x.custtransId==id).FirstOrDefaultAsync();
            ViewData["transactionId"] = new SelectList(_context.Transactions, "transactionId", "transactionName", custTrans.transactionId);
            custTrans.paperlist = custTrans.paperlist.Where(x => x.isdel == false).ToList();
            return PartialView(custTrans);
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Edit(CustTrans custTrans)
        {
            var olddata = _context.CustTrans.Where(x => x.custtransId == custTrans.custtransId).FirstOrDefault();
            custTrans.date = olddata.date;
            if (olddata.end == true && custTrans.end == true)
                custTrans.enddate = olddata.enddate;
            if (custTrans.end)
                custTrans.enddate = date();

            if (custTrans.paperlist != null)
            {
                int index = 1;
                foreach (var item in custTrans.paperlist)
                {
                    if (item.paperdataId == 0)
                    {
                        if (_context.paperDatas.Count() == 0)
                            item.paperdataId = index++;
                        else
                            item.paperdataId = index++ + _context.paperDatas.Max(x => x.paperdataId);
                        item.custtransId = custTrans.custtransId;
                        _context.Add(item);
                    }
                    else
                        _context.Update(item);
                    _context.SaveChanges();
                }
                custTrans.paperlist = null;
            }
                if (custTrans.paid != 0)
                {
                    PayMent p = new PayMent();
                    if (_context.payMents.Count() == 0)
                        p.paymentId = 1;
                    else
                        p.paymentId = 1 + _context.payMents.Max(x => x.paymentId);
                    p.paid = custTrans.paid;
                    p.date = date();
                    p.custtransId = custTrans.custtransId;
                    p.isdel = false;
                    _context.Add(p);
                    _context.SaveChanges();
                    custTrans.paid = _context.payMents.Where(x => x.custtransId == custTrans.custtransId).Sum(x => x.paid);
                    custTrans.change = custTrans.totalmoney - custTrans.paid;
                }
                _context.Entry(olddata).CurrentValues.SetValues(custTrans);
                 _context.SaveChanges();
            backup();
            return Ok();
        }
        [Produces("application/json")]
        public IActionResult ListTrans()
        {
            var data = _context.Transactions.Where(x=>x.isdel==false).ToList();
            return Ok(data);
        }

        // GET: CustTrans/Delete/5
        // POST: CustTrans/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var custTrans = await _context.CustTrans.FindAsync(id);
                custTrans.isdel = true;
                _context.Update(custTrans);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok(null);
        }
        [Produces("application/json")]
        public IActionResult GetIdForPaper(String paperName)
        {
            var paperid = _context.papers.Where(x => x.paperName == paperName).FirstOrDefault();
            if(paperid!=null)
            return Ok(paperid.paperId);
            else
                return Ok();
        }
        [Produces("application/json")]
        public IActionResult AutoComplete(String search)
        {
            var data = _context.papers.Where(x => x.paperName.Contains(search)).Select(x => new
            {
                itemId = x.paperId,
                itemName = x.paperName
            }).ToList();
            return Ok(data);
        }
        [Produces("application/json")]
        public IActionResult DeleteEdit(int id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id != 0)
                {
                    var paperdata = _context.paperDatas.Find(id);
                    paperdata.isdel = true;
                    _context.Update(paperdata);
                    _context.SaveChanges();
                    return Ok();
                }
            }
            return Ok(null);
        }
        public IActionResult PerDelete(int id)
        {
            var custtrans = _context.CustTrans.Where(x => x.custtransId == id).FirstOrDefault();
            _context.Remove(custtrans);
            var paperdata = _context.paperDatas.Where(x => x.custtransId == custtrans.custtransId).ToList();
            _context.RemoveRange(paperdata);
            if (_context.CustTrans.Where(x => x.customerId == custtrans.customerId).Count() == 1)
            {
                var customer = _context.CustTrans.Where(x => x.customerId == custtrans.customerId).FirstOrDefault();
                _context.Remove(customer);
            }
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult restore(int id)
        {
            var custtrans = _context.CustTrans.Where(x => x.custtransId == id).FirstOrDefault();
            custtrans.isdel = false;
            _context.Update(custtrans);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult PrintTransaction(int id)
        {
            var paper = _context.CustTrans.Where(x => x.custtransId == id).Include(x => x.Customer).Include(x => x.Transaction).Include(x => x.paperlist).Include(x => x.paymentlist).FirstOrDefault();
            return View(paper);
        }
        public void backup()
        {
            if (!System.IO.Directory.Exists(@"D:\data"))
                System.IO.Directory.CreateDirectory(@"D:\data");
            if (!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString());
            if (!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString());
            if(!System.IO.Directory.Exists(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString()+"\\"+DateTime.Now.Day.ToString()))
                System.IO.Directory.CreateDirectory(@"D:\data\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString());
            string filename = String.Format("{0}__{1}", customedate(), ".bak" + "'");
            _context.Database.ExecuteSqlCommand("BACKUP DATABASE Eng TO  DISK ='D://data/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + filename);
        }
        public String date()
        {
            var hour = DateTime.Now.Hour;
            var datenow = DateTime.Now;
            String date = null;
            if (hour > 12)
                date = datenow.Year + "-" + datenow.Month + "-" + datenow.Day + "  " + (hour - 12) + ":" + datenow.Minute + " م";
            else if (hour == 12)
                date = datenow.Year + "-" + datenow.Month + "-" + datenow.Day + "  " + hour + ":" + datenow.Minute + " م";
            else
                date = datenow.Year + "-" + datenow.Month + "-" + datenow.Day + "  " + hour + ":" + datenow.Minute + " ص";
            return date;
        }
        public String customedate()
        {
            var hour = DateTime.Now.Hour;
            var datenow = DateTime.Now;
            String date = null;
            if (hour > 12)
                date =(hour - 12) + "--" + datenow.Minute + "-" + datenow.Second + " م";
            else if (hour == 12)
                date = hour + "--" + datenow.Minute + "-" + datenow.Second + " م";
            else
                date =  hour + "--" + datenow.Minute + "-" + datenow.Second + " ص";
            return date;
        }
    }
}
