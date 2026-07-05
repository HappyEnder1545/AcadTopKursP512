namespace PersonalLibrary.Models
{
    public enum BookStatus
    {
        NotRead,
        Reading,
        Read
    }

    public static class BookStatusHelper
    {
        public static string ToRu(this BookStatus s) => s switch
        {
            BookStatus.NotRead => "Не прочитана",
            BookStatus.Reading => "Читается",
            BookStatus.Read => "Прочитана",
            _ => s.ToString()
        };

        public static BookStatus Parse(string? value)
        {
            return value switch
            {
                "Reading" => BookStatus.Reading,
                "Read" => BookStatus.Read,
                _ => BookStatus.NotRead
            };
        }
    }
}