using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class NoteFormObject
    {
        public NoteFormObject()
        { }
        public NoteFormObject(Note note)
        {
            Name = note.Name;
            Body = note.Body;
        }
        [Required]
        public string Name { get; set; }
        public string Body { get; set; }

        public Note ToNote()
        {
            return new Note
            {
                Name = Name,
                Body = Body
            };
        }
    }
}
