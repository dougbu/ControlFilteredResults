using System.ComponentModel.DataAnnotations;

namespace ControlFilteredResults.Models
{
    public class File
    {
        public int FileListId { get; set; }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
