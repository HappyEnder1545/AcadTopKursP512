namespace PersonalLibrary.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string FullName => $"{FirstName} {LastName}".Trim();
        public override string ToString() => FullName;
    }
}