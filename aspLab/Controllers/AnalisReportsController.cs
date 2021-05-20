using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabDB.Data;
using aspLab.Models;

namespace aspLab.Controllers
{
    public class AnalisReportsController : Controller
    {
        private readonly LabDBContext _context;

        public AnalisReportsController(LabDBContext context)
        {
            _context = context;
        }

        // GET: AnalisReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnalisReport.ToListAsync());
        }

        // GET: AnalisReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analisReport = await _context.AnalisReport
                .FirstOrDefaultAsync(m => m.id == id);
            if (analisReport == null)
            {
                return NotFound();
            }

            return View(analisReport);
        }

        // GET: AnalisReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnalisReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,object_name,duration_h,result,dateTime")] AnalisReport analisReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analisReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analisReport);
        }

        // GET: AnalisReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analisReport = await _context.AnalisReport.FindAsync(id);
            if (analisReport == null)
            {
                return NotFound();
            }
            return View(analisReport);
        }

        // POST: AnalisReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,object_name,duration_h,result,dateTime")] AnalisReport analisReport)
        {
            if (id != analisReport.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analisReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalisReportExists(analisReport.id))
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
            return View(analisReport);
        }

        // GET: AnalisReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analisReport = await _context.AnalisReport
                .FirstOrDefaultAsync(m => m.id == id);
            if (analisReport == null)
            {
                return NotFound();
            }

            return View(analisReport);
        }

        // POST: AnalisReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var analisReport = await _context.AnalisReport.FindAsync(id);
            _context.AnalisReport.Remove(analisReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalisReportExists(int id)
        {
            return _context.AnalisReport.Any(e => e.id == id);
        }
    }
}
