namespace NewsBlogAPI.DTO
{
    public class NewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }

        public int AuthorId { get; set; }
    }
}
