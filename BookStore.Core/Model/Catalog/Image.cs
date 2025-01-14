using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Catalog;

public class Image
{
    private Image(string name, Guid bookId)
    {
        Name = name;
        BookId = bookId;
    }
    public Guid BookId { get; }
    public string Name { get; }
    public string Url { get; private set; }

    public static Result<Image> CreateImage(string url, Guid bookId)
    {
        return Result.Success(new Image(url, bookId));
    }
    public Image SetImageLink(string url)
    {
        Url = url;
        return this;
    }
    
}