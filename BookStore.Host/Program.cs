using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql;
using BookStore.PostgreSql.Mapper;
using BookStore.PostgreSql.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddAutoMapper(typeof(AuthorProfile));
builder.Services.AddScoped<BookProfile>(sp =>
{
    var dbContext = sp.GetRequiredService<BookStoreDbContext>();
    return new BookProfile(dbContext);
});
builder.Services.AddAutoMapper(typeof(BookProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile));

builder.AddNpgsqlDbContext<BookStoreDbContext>("BookStoreDb");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();