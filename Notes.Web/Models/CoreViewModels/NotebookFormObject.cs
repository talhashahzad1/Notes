using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class NotebookFormObject
    {
        public NotebookFormObject()
        { }

        public NotebookFormObject(Notebook notebook)
        {
            Name = notebook.Name;
            Description = notebook.Description;
            ParentId = notebook.ParentId;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string TagList { get; set; }

        public Notebook ToNotebook()
        {
            return new Notebook
            {
                Name = Name,
                Description = Description,
                ParentId = ParentId,
                UpdatedAt = DateTime.Now
            };
        }

        public Notebook CreateNotebook(ApplicationUser user)
        {
            var notebook = ToNotebook();
            notebook.ApplicationUser = user;
            return notebook;
        }

        public void UpdateNotebook(Notebook notebook)
        {
            notebook.UpdatedAt = DateTime.Now;
            notebook.Name = Name;
            notebook.Description = Description;
            notebook.ParentId = ParentId;
        }
    }
}
