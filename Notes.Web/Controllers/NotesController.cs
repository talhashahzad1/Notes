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
using Notes.Web.Models.CoreViewModels;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;    
        }
        
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notes.Include(n => n.Notebook);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Notebook)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int notebookId, NoteFormObject nfo)
        {
            if (ModelState.IsValid)
            {
                var note = nfo.ToNote();
                note.NotebookId = notebookId;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { NotebookId = notebookId, Id = note.Id });
            }
            return View(nfo);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { Id = note.Id });
            }
            return View(note);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Notebook)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Notebooks", new { Id = note.NotebookId });
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
