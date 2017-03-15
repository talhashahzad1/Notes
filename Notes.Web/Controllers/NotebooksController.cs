using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Web.Data;
using Notes.Web.Models;

namespace Notes.Web.Controllers
{
    public class NotebooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotebooksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Notebooks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notebooks.Include(n => n.ApplicationUser).Include(n => n.Parent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Notebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebooks
                .Include(n => n.ApplicationUser)
                .Include(n => n.Parent)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (notebook == null)
            {
                return NotFound();
            }

            return View(notebook);
        }

        // GET: Notebooks/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId");
            return View();
        }

        // POST: Notebooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedAt,UpdatedAt,ApplicationUserId,ParentId")] Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notebook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", notebook.ApplicationUserId);
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", notebook.ParentId);
            return View(notebook);
        }

        // GET: Notebooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebooks.SingleOrDefaultAsync(m => m.Id == id);
            if (notebook == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", notebook.ApplicationUserId);
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", notebook.ParentId);
            return View(notebook);
        }

        // POST: Notebooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt,UpdatedAt,ApplicationUserId,ParentId")] Notebook notebook)
        {
            if (id != notebook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotebookExists(notebook.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", notebook.ApplicationUserId);
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", notebook.ParentId);
            return View(notebook);
        }

        // GET: Notebooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebooks
                .Include(n => n.ApplicationUser)
                .Include(n => n.Parent)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (notebook == null)
            {
                return NotFound();
            }

            return View(notebook);
        }

        // POST: Notebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notebook = await _context.Notebooks.SingleOrDefaultAsync(m => m.Id == id);
            _context.Notebooks.Remove(notebook);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NotebookExists(int id)
        {
            return _context.Notebooks.Any(e => e.Id == id);
        }
    }
}
