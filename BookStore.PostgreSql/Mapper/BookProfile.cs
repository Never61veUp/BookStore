using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;

namespace BookStore.PostgreSql.Mapper;

public class BookProfile : Profile
{
    public BookProfile()
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
                ).Value);

        CreateMap<Book, BookEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StockCount, opt => opt.MapFrom(src => src.StockCount))
            .ForPath(dest => dest.Author, opt => opt.Ignore())
            .ForPath(dest => dest.Category, opt => opt.Ignore());
    }
}