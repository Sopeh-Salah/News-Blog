using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewsBlogAPI.Helpers;

namespace NewsBlogAPI.Models
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Publication date is required.")]
        [ValidatePublicationDate(ErrorMessage = "Publication date must be between today and a week from today.")]
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author author { get; set; }
    }
}
