﻿using BookStore.Core.Model.Catalog;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Abstractions;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Result<Book>> GetBookByIdAsync(Guid id);
    Task<List<Book>> GetBooksByCategory(Guid categoryId);
    Task<Result> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(Guid id, Book book);
    Task<bool> DeleteBookAsync(Guid id);

}