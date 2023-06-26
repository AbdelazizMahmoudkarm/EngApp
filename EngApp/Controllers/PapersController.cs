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
    public class PapersController : Controller
    {
        private readonly AppDb _context;

        public PapersController(AppDb context)
        {
            _context = context;
        }

        // GET: Papers
        public async Task<IActionResult> Index(int? pageNumber)
        {
            return View(await PaginatedList<Paper>.CreateAsync(_context.papers.Where(x=>x.isdel==false),pageNumber ??1 ,9));
        }
        // GET: Papers/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Papers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("paperId,paperName,isdel")] Paper paper)
        {
            if (_context.papers.Count() == 0)
                paper.paperId = 1;
            else
                paper.paperId = 1 + _context.papers.Max(x => x.paperId);
            paper.isdel = false;
            if (ModelState.IsValid)
            {
                
                _context.Add(paper);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok(null);
        }

        // GET: Papers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var paper = await _context.papers.FindAsync(id);
            return PartialView(paper);
        }

        // POST: Papers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("paperId,paperName,isdel")] Paper paper)
        {
         
            if (ModelState.IsValid)
            {
                    _context.Update(paper);
                    await _context.SaveChangesAsync();
            return Ok();
            }
           return Ok(null);
        }


        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paper = await _context.papers.FindAsync(id);
            paper.isdel = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaperExists(int id)
        {
            return _context.papers.Any(e => e.paperId == id);
        }
    }
}
