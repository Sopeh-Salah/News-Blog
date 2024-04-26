using System.ComponentModel.DataAnnotations;

namespace NewsBlogAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Author's name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Author's name must be between 3 and 20 characters.")]
        public string Name { get; set; }
        public ICollection<New> news { get; set; }
    }
}
