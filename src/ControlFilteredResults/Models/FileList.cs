using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ControlFilteredResults.Models
{
    public class FileList
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public List<File> FileNames { get; set; }

        public IFormFileCollection Files { get; set; }
    }
}
