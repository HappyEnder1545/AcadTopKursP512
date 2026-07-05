namespace PersonalLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int? Year { get; set; }
        public int? Pages { get; set; }
        public BookStatus Status { get; set; } = BookStatus.NotRead;
        public string? Notes { get; set; }

        // поля для отображения в таблице
        public string AuthorName { get; set; } = "";
        public string GenreName { get; set; } = "";
        public string StatusRu => Status.ToRu();
    }
}