using Microsoft.EntityFrameworkCore;
using Notes.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    /// <summary>
    /// Collects records associated with selected tag.
    /// Note the records will only have id and names available, unless modified by some other process.
    /// Construction only accessibly through 'WithAssociations' method.
    /// </summary>
    public class TagViewModel
    {
        private TagViewModel(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            Notes = new List<Note>();
            Notebooks = new List<Notebook>();
        }
        private TagViewModel(IEnumerable<TagItemView> tagItemViews)
        {
            Id = tagItemViews.First().TagId;
            Name = tagItemViews.First().TagName;
            Notes = tagItemViews.Where(tiv => tiv.ItemType == ItemType.Note)
                .Select(tiv => new Note { Id = tiv.ItemId, Name = tiv.ItemName });
            Notebooks = tagItemViews.Where(tiv => tiv.ItemType == ItemType.Notebook)
                .Select(tiv => new Notebook { Id = tiv.ItemId, Name = tiv.ItemName });
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Note> Notes { get; set; }
        public IEnumerable<Notebook> Notebooks { get; set; }

        public static async Task<TagViewModel> WithAssociations(int tagId, ApplicationDbContext db)
        {
            var tagItemViews = await db.TagItemViews.Where(tiv => tiv.TagId == tagId).ToListAsync();
            if (tagItemViews.Count == 0)
            {
                var tag = await db.Tags.SingleOrDefaultAsync(t => t.Id == tagId);
                return new TagViewModel(tag);
            }
            return new TagViewModel(tagItemViews);
        }
    }
}
