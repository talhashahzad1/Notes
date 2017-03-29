using Notes.Web.Models.CoreViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    public class Note
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Body { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int NotebookId { get; set; }
        public virtual Notebook Notebook { get; set; }
    }
}
