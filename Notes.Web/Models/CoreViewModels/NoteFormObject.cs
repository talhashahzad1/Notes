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

        public NoteFormObject(Note note, IList<string> tagList) : this(note)
        {
            if (tagList.Count > 0)
            {
                TagList = string.Join(", ", tagList);
            }
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Body { get; set; }
        public string TagList { get; set; }

        public Note ToNote()
        {
            return new Note
            {
                Name = Name,
                Body = Body
            };
        }

        public Note CreateNote()
        {
            return new Note
            {
                Name = Name,
                Body = Body,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public void UpdateNote(Note note)
        {
            note.Name = Name;
            note.Body = Body;
            note.UpdatedAt = DateTime.Now;
        }
    }
}
