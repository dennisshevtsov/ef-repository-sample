// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Book
{
  using EfRepositorySample.Book;

  /// <summary>Represents a book entity.</summary>
  public sealed class BookEntity : EntityBase, IBookEntity
  {
    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookEntity"/> class.</summary>
    public BookEntity()
    {
      Title       = string.Empty;
      Description = string.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookEntity"/> class.</summary>
    /// <param name="bookEntity">An object that represents a book entity.</param>
    public BookEntity(IBookEntity bookEntity) : this()
    {
      BookId      = bookEntity.BookId;
      Title       = bookEntity.Title;
      Description = bookEntity.Description;
      Pages       = bookEntity.Pages;
    }

    /// <summary>Gets an object that represents an ID of a book.</summary>
    public Guid BookId { get => Id; set => Id = value; }

    /// <summary>Gets an object that represents a title of a book.</summary>
    public string Title { get; }

    /// <summary>Gets an object that represents a description of a book.</summary>
    public string Description { get; }

    /// <summary>Gets an object that represents a number of pages of a book.</summary>
    public int Pages { get; }
  }
}
