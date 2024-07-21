namespace BookStore.Core.Models
{
    public class Book
    {
        public const int MAX_TITLE_LENGTH = 20;
        public const int MAX_DESCRIPTION_LENGTH = 150;

        private Book(Guid id, string title, string description, decimal price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public decimal Price { get; }

        public static (Book book, string error) Create(Guid id, string title, string description, decimal price)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                errors.Add($"Title cannot be empty or longer than {MAX_TITLE_LENGTH} characters.");
            }

            if (string.IsNullOrEmpty(description) || description.Length > MAX_DESCRIPTION_LENGTH)
            {
                errors.Add($"Description cannot be empty or longer than {MAX_DESCRIPTION_LENGTH} characters.");
            }

            if (price < 0)
            {
                errors.Add("Price cannot be negative.");
            }

            if (errors.Count > 0)
            {
                return (null, string.Join("; ", errors));
            }

            var book = new Book(id, title, description, price);
            return (book, string.Empty);
        }
    }
}