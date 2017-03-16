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

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public Notebook ToNotebook()
        {
            return new Notebook
            {
                Name = Name,
                Description = Description,
                ParentId = ParentId
            };
        }
    }
}
