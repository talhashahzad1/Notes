using Microsoft.EntityFrameworkCore;
using Notes.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class TagViewModel
    {
        private readonly ApplicationDbContext _db;

        private TagViewModel(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; }
        public ICollection<Notebook> Notebooks { get; set; }

        public static async Task<TagViewModel> WithAssociations(Tag tag, ApplicationDbContext db)
        {
            if (tag == null)
            {
                return null;
            }

            var tvm = new TagViewModel(tag);
            var tagItems = await db.TagItems.Where(ti => ti.TagId == tvm.Id).ToListAsync();

            tvm.Notes = await db.Notes.Where(n => tagItems
                .Where(ti => ti.ItemType == ItemType.Note).Select(ti => ti.ItemId)
                .Contains(n.Id)).ToListAsync();

            tvm.Notebooks = await db.Notebooks.Where(n => tagItems
                .Where(ti => ti.ItemType == ItemType.Notebook).Select(ti => ti.ItemId)
                .Contains(n.Id)).ToListAsync();

            return tvm;
        }

        public static async Task<TagViewModel> WithAssociations(int tagId, ApplicationDbContext db)
        {
            var tag = await db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

            return await WithAssociations(tag, db);
        }
    }
}
