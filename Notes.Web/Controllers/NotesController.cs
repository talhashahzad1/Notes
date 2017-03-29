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
using Markdig;
using Notes.Web.Services;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GutenTag _gutenTag;

        public NotesController(ApplicationDbContext context, GutenTag gutenTag)
        {
            _context = context;
            _gutenTag = gutenTag;
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
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            note.Body = Markdown.ToHtml(note.Body, pipeline);
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
                var note = nfo.CreateNote();
                note.NotebookId = notebookId;
                _context.Add(note);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(nfo.TagList))
                {
                    await _gutenTag.AddToDb(new TagHolder(note.Id, "Note", nfo.TagList));
                }
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
            var tagList = await _context.TagItems.Include(ti => ti.Tag)
                .Where(ti => ti.ItemId == note.Id && ti.ItemType == ItemType.Note).ToListAsync();
            return View(new NoteFormObject(note, tagList.Select(ti => ti.Tag.Name).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteFormObject nfo)
        {
            var note = await _context.Notes.FindAsync(id);

            if (ModelState.IsValid)
            {
                nfo.UpdateNote(note);
                _context.Update(note);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(nfo.TagList))
                {
                    await _gutenTag.AddToDb(new TagHolder(note.Id, "Note", nfo.TagList));
                }
                return RedirectToAction("Details", new { Id = note.Id });
            }
            return View(nfo);
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
