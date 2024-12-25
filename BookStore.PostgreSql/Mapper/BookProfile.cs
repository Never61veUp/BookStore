using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;

namespace BookStore.PostgreSql.Mapper;

public class BookProfile : Profile
{
    public BookProfile(BookStoreDbContext context)
    {
        CreateMap<BookEntity, Book>()
            .ConstructUsing(entity => 
                Book.Create(
                    entity.Id,
                    entity.Title,
                    entity.Description,
                    entity.Price,
                    entity.Author.Id,
                    entity.Category.Id,
                    entity.StockCount
                ).Value)
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));
        
        CreateMap<Book, BookEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StockCount, opt => opt.MapFrom(src => src.StockCount))
            .ForPath(dest => dest.Author, opt => opt.Ignore()) // Игнорируем Author, чтобы заполнить его вручную
            .ForPath(dest => dest.Category, opt => opt.Ignore()) // То же для Category
            .AfterMap((src, dest) =>
            {
                    dest.Author = context.Authors.Find(src.AuthorId);
                    dest.Category = context.Categories.Find(src.CategoryId);
            });
    }
}