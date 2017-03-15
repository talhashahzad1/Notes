using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    // Ahhh... No integrity!
    public class TagItem
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        [Required]
        public int ItemId { get; set; }
        [Required]
        public ItemType ItemType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }

    public enum ItemType
    {
        Asset, Note, Notebook
    }
}
