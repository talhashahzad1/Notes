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
using Notes.Web.Data.Repositories;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class NotebooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NotebookRepository _repo;

        public NotebooksController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, NotebookRepository repo)
        {
            _context = context;
            _repo = repo;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index(string order = "UpdatedAt", string direction = "DESC", int take = 20, int skip = 0)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await _repo.List(user, new ListOptions(order, direction, take, skip)));
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebooks.Include(n => n.Notes).SingleOrDefaultAsync(m => m.Id == id);
            if (notebook == null)
            {
                return NotFound();
            }

            return View(notebook);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotebookFormObject nfo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var notebook = nfo.CreateNotebook(user);
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
            return View(new NotebookFormObject(notebook));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NotebookFormObject nfo)
        {
            if (ModelState.IsValid)
            {
                var notebook = await _context.Notebooks.SingleOrDefaultAsync(n => n.Id == id);
                if (notebook == null)
                {
                    return NotFound();
                }

                nfo.UpdateNotebook(notebook);
                _context.Update(notebook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = notebook.Id });
            }
            return View(nfo);
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
