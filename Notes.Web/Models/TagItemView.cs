using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    // SQL View combining tags with associated items
    public class TagItemView
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemType ItemType { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string ItemName { get; set; }
    }
}
