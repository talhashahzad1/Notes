using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Models.CoreViewModels
{
    public class AssetFormObject
    {
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
