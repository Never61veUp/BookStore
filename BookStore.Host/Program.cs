using BookStore.Application.Abstractions;
using BookStore.Application.Services;
using BookStore.Auth.Abstractions;
using BookStore.Auth.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Host.Extensions;
using BookStore.PostgreSql;
using BookStore.PostgreSql.Mapper;
using BookStore.PostgreSql.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddOpenApi();


// var authConfig = configuration.GetSection(nameof(AuthorizationOptions));
// services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddScoped<IAuthorRepository, AuthorRepository>();
services.AddScoped<IAuthorService, AuthorService>();
services.AddScoped<IBookRepository, BookRepository>();
services.AddScoped<IBookService, BookService>();
services.AddScoped<ICategoriesRepository, CategoriesRepository>();
services.AddScoped<ICategoriesService, CategoriesService>();
services.AddScoped<ICartService, CartService>();
services.AddScoped<ICartRepository, CartRepository>();

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
services.AddDynamicPermissionPolicies();

builder.AddNpgsqlDbContext<BookStoreDbContext>("BookStoreDb", options =>
{
    options.DisableHealthChecks = true;
    options.DisableTracing = true;
});

services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowCredentials()
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