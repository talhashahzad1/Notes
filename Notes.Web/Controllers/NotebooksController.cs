using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Web.Data;
using Notes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Notes.Web.Models.CoreViewModels;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class NotebooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotebooksController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notebooks.Include(n => n.ApplicationUser).Include(n => n.Parent);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
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
        
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotebookFormObject nfo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var notebook = nfo.ToNotebook();
                notebook.ApplicationUser = user;
                _context.Add(notebook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", nfo.ParentId);
            return View(nfo);
        }
        
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
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", notebook.ParentId);
            return View(notebook);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notebook notebook)
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
            ViewData["ParentId"] = new SelectList(_context.Notebooks, "Id", "ApplicationUserId", notebook.ParentId);
            return View(notebook);
        }
        
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
