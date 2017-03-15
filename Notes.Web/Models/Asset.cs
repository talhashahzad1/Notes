using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    public class Asset
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}
