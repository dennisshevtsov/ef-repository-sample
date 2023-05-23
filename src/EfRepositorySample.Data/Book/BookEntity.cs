// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Book
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Author;

  /// <summary>Represents a book entity.</summary>
  public sealed class BookEntity : EntityBase, IBookEntity, IUpdatable<IBookEntity>
  {
    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookEntity"/> class.</summary>
    public BookEntity()
    {
      Title       = string.Empty;
      Description = string.Empty;
      BookAuthors = new List<AuthorEntity>();
    }

    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookEntity"/> class.</summary>
    /// <param name="bookEntity">An object that represents a book identity.</param>
    public BookEntity(IBookIdentity bookIdentity) : this()
    {
      BookId = bookIdentity.BookId;
    }

    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookEntity"/> class.</summary>
    /// <param name="bookEntity">An object that represents a book entity.</param>
    public BookEntity(IBookEntity bookEntity) : this((IBookIdentity)bookEntity)
    {
      Title       = bookEntity.Title;
      Description = bookEntity.Description;
      Pages       = bookEntity.Pages;
      BookAuthors = AuthorEntity.Copy(bookEntity.Authors);
    }

    /// <summary>Gets an object that represents an ID of a book.</summary>
    public Guid BookId { get => Id; set => Id = value; }

    /// <summary>Gets an object that represents a title of a book.</summary>
    public string Title { get; }

    /// <summary>Gets an object that represents a description of a book.</summary>
    public string Description { get; }

    /// <summary>Gets an object that represents a number of pages of a book.</summary>
    public int Pages { get; }

    /// <summary>Gets an object that represents a collection of authors of this book.</summary>
    public IEnumerable<IAuthorEntity> Authors => BookAuthors;

    public ICollection<AuthorEntity> BookAuthors { get; }

    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    public void Update(IBookEntity newEntity) => base.Update(newEntity);

    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    public void Update(IBookEntity newEntity, IEnumerable<string> properties) =>
       base.Update(newEntity, properties);

    /// <summary>Copies a collection of books.</summary>
    /// <param name="books">An object that represents a collection of books to copy.</param>
    /// <returns>An object that represents a copied collection of books.</returns>
    public static ICollection<BookEntity> Copy(IEnumerable<IBookEntity> books)
      => books.Select(entity => new BookEntity(entity))
              .ToList();
  }
}
