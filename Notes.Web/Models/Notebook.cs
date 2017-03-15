using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    public class Notebook
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int? ParentId { get; set; }
        public Notebook Parent { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
