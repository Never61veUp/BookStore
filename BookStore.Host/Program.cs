using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Host.Extensions;
using BookStore.PostgreSql;
using BookStore.PostgreSql.Mapper;
using BookStore.PostgreSql.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddOpenApi();

services.AddScoped<IAuthorRepository, AuthorRepository>();
services.AddScoped<IAuthorService, AuthorService>();
services.AddScoped<IBookRepository, BookRepository>();
services.AddScoped<IBookService, BookService>();
services.AddScoped<ICategoriesRepository, CategoriesRepository>();
services.AddScoped<ICategoriesService, CategoriesService>();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserService, UserService>();

services.AddScoped<IYandexStorageService, YandexStorageService>();
services.AddScoped<IImageService, ImageService>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.AddApiAuthentication(configuration);

services.AddAutoMapper(typeof(AuthorProfile));
services.AddAutoMapper(typeof(BookProfile));
services.AddAutoMapper(typeof(CategoryProfile));

builder.AddNpgsqlDbContext<BookStoreDbContext>("BookStoreDb");

services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();