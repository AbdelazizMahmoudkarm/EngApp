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
    public class TransactionsController : Controller
    {
        private readonly AppDb _context;

        public TransactionsController(AppDb context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int? pageNumber)
        {
            return View(await PaginatedList<Transaction>.CreateAsync(_context.Transactions.Where(x=>x.isdel==false),pageNumber ??1,9));
        }
        // GET: Transactions/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Create([Bind("transactionId,transactionName")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (_context.Transactions.Count() == 0)
                    transaction.transactionId = 1;
                else
                    transaction.transactionId = 1 + _context.Transactions.Max(x => x.transactionId);
                _context.Add(transaction);
                    await _context.SaveChangesAsync();
                
                return Ok();
            }
            return Ok(null);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return PartialView(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Edit([Bind("transactionId,transactionName")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.transactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            transaction.isdel = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.transactionId == id);
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
    }
}
