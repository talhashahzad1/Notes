using Microsoft.EntityFrameworkCore;
using Notes.Web.Data;
using Notes.Web.Models;
using Notes.Web.Models.CoreViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Services
{
    public class GutenTag
    {
        private readonly ApplicationDbContext _db;
        private TagHolder _tagHolder { get; set; }

        public GutenTag(ApplicationDbContext db)
        {
            _db = db;
        }

        // foreach tag name in tagholder.TagNames, if the tag exists, create a new tag item for it.
        // If the tag doesn't exist, create the tag and then add a tag item.
        public async Task AddToDb(TagHolder tagHolder)
        {
            _tagHolder = tagHolder;
            // Make one trip to the database
            var existingTags = await _db.TagItems
                .Where(ti => ti.ItemId == _tagHolder.ItemId && ti.ItemType == _tagHolder.ItemType).ToListAsync();
            // Keep a list of tags saved so we know what to delete later
            var tagsOnItem = new List<TagItem>();
            foreach (var name in _tagHolder.TagNames)
            {
                var tag = await MatchOrMakeTag(name);
                // Query from tags in memory.
                var match = await MatchOrMakeTagItem(tag, existingTags);
                tagsOnItem.Add(match);
            }
            var unmatchedItems = existingTags.Except(tagsOnItem);
            _db.TagItems.RemoveRange(unmatchedItems);
            await _db.SaveChangesAsync();
        }

        private async Task<Tag> MatchOrMakeTag(string name)
        {
            var tag = _db.Tags.Where(t => t.Name.ToLower() == name.ToLower()).SingleOrDefault();
            // Create new tag if necessary
            if (tag == null)
            {
                tag = new Tag { Name = name };
                _db.Tags.Add(tag);
                await _db.SaveChangesAsync();
            }
            return tag;
        }

        private async Task<TagItem> MatchOrMakeTagItem(Tag tag, List<TagItem> existingTagItems)
        {
            var match = existingTagItems.Where(ti => ti.TagId == tag.Id).FirstOrDefault();
            if (match == null)
            {
                // Make tag item
                match = new TagItem { ItemId = _tagHolder.ItemId, ItemType = _tagHolder.ItemType, TagId = tag.Id, CreatedAt = DateTime.Now };
                _db.TagItems.Add(match);
                await _db.SaveChangesAsync();
            }
            return match;
        }
    }
}
