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
    public class UserExceptionsController : Controller
    {
        private readonly LabDBContext _context;

        public UserExceptionsController(LabDBContext context)
        {
            _context = context;
        }

        // GET: UserExceptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserException.ToListAsync());
        }

        // GET: UserExceptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userException = await _context.UserException
                .FirstOrDefaultAsync(m => m.id == id);
            if (userException == null)
            {
                return NotFound();
            }

            return View(userException);
        }

        // GET: UserExceptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserExceptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,project_name,error_text,dateTime")] UserException userException)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userException);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userException);
        }

        // GET: UserExceptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userException = await _context.UserException.FindAsync(id);
            if (userException == null)
            {
                return NotFound();
            }
            return View(userException);
        }

        // POST: UserExceptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,project_name,error_text,dateTime")] UserException userException)
        {
            if (id != userException.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userException);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExceptionExists(userException.id))
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
            return View(userException);
        }

        // GET: UserExceptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userException = await _context.UserException
                .FirstOrDefaultAsync(m => m.id == id);
            if (userException == null)
            {
                return NotFound();
            }

            return View(userException);
        }

        // POST: UserExceptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userException = await _context.UserException.FindAsync(id);
            _context.UserException.Remove(userException);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExceptionExists(int id)
        {
            return _context.UserException.Any(e => e.id == id);
        }
    }
}
