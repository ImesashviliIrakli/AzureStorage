using System.ComponentModel.DataAnnotations;

namespace BlobStorageProject.Models
{
    public class Container
    {
        [Required]
        public string? Name { get; set; }
    }
}
