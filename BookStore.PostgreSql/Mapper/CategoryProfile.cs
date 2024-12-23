using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;

namespace BookStore.PostgreSql.Mapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryEntity, Category>()
            .ConstructUsing(entity =>
                Category.Create(
                    entity.Id,
                    entity.Name,
                    entity.ParentCategory.Id
                ).Value);
        
        CreateMap<Category, CategoryEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore()); // Обработка ParentCategory отдельно
    }
}