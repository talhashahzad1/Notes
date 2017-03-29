using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class TagHolder
    {
        public TagHolder(int itemId, string typeName, string tagNames)
        {
            ItemId = itemId;
            ItemType itemType;
            if (Enum.TryParse<ItemType>(typeName, out itemType))
            {
                ItemType = itemType;
            }
            MakeList(tagNames);
        }

        public int ItemId { get; private set; }
        public ItemType ItemType { get; private set; }
        public List<string> TagNames { get; private set; } = new List<string>();

        private void MakeList(string tagNames)
        {
            if (tagNames == null)
            {
                return;
            }

            foreach (var tag in tagNames.Split(','))
            {
                TagNames.Add(tag.Trim());
            }
        }
    }
}
