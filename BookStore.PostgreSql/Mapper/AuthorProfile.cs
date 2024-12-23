using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;

namespace BookStore.PostgreSql.Mapper;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<AuthorEntity, Author>()
            .ConstructUsing(entity =>
                Author.Create(
                    entity.Id,
                    entity.FullName,
                    entity.BirthDate ?? DateTime.MinValue,
                    entity.Biography ?? string.Empty
                ).Value);

        CreateMap<Author, AuthorEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography));
    }
}