using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class TagFormObject
    {
        public TagFormObject()
        {

        }

        public TagFormObject(Tag tag)
        {
            Name = tag.Name;
        }

        [Required]
        public string Name { get; set; }

        public Tag ToTag()
        {
            return new Tag
            {
                Name = Name
            };
        }

        public Tag CreateTag()
        {
            return new Tag
            {
                Name = Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public void UpdateTag(Tag tag)
        {
            tag.Name = Name;
            tag.UpdatedAt = DateTime.Now;
        }
    }
}
