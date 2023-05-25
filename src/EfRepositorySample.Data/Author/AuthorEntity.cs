// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Author
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Book;

  /// <summary>Represents an author entity.</summary>
  public sealed class AuthorEntity : EntityBase, IAuthorEntity, IUpdatable<IAuthorEntity>
  {
    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Author.AuthorEntity"/> class.</summary>
    public AuthorEntity()
    {
      Name = string.Empty;
      Bio = string.Empty;
      AuthorBooks = new List<BookEntity>();
    }

    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Author.AuthorEntity"/> class.</summary>
    /// <param name="authorIdentity">An object that represents an author identity.</param>
    public AuthorEntity(IAuthorIdentity authorIdentity) : this()
    {
      AuthorId = authorIdentity.AuthorId;
    }

    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Author.AuthorEntity"/> class.</summary>
    /// <param name="authorEntity">An object that represents an author entity.</param>
    public AuthorEntity(IAuthorEntity authorEntity) : this((IAuthorIdentity)authorEntity)
    {
      Name = authorEntity.Name;
      Bio = authorEntity.Bio;
      AuthorBooks = BookEntity.Copy(authorEntity.Books);
    }

    /// <summary>Gets an object that represents an ID of an author.</summary>
    public Guid AuthorId { get => Id; private set => Id = value; }

    /// <summary>Gets an object that represents a name of an author.</summary>
    public string Name { get; set; }

    /// <summary>Gets an object that represents a bio of an author.</summary>
    public string Bio { get; set; }

    /// <summary>Gets an object that represents a collection of this author's books.</summary>
    public IEnumerable<IBookEntity> Books
    {
      get => AuthorBooks;
      set => AuthorBooks = value.Select(entity => EntityBase.Create<IBookEntity, BookEntity>(entity))
                                .ToList();
    }

    /// <summary>Gets an object that represents a collection of this author's books.</summary>
    public ICollection<BookEntity> AuthorBooks { get; set; }

    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    public void Update(IAuthorEntity newEntity) => base.Update(newEntity);

    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    public void Update(IAuthorEntity newEntity, IEnumerable<string> properties) =>
       base.Update(newEntity, properties);

    /// <summary>Copies a collection of authors.</summary>
    /// <param name="authors">An object that represents a collection of authors to copy.</param>
    /// <returns>An object that represents a copied collection of authors.</returns>
    public static ICollection<AuthorEntity> Copy(IEnumerable<IAuthorEntity> authors)
      => authors.Select(entity => new AuthorEntity(entity))
                .ToList();

    protected override void Update(object newEntity, string property)
    {
      if (property == nameof(Books))
      {
        var newAuthorEntity = (IAuthorEntity)newEntity;

        var newBooks = newAuthorEntity.Books.Select(entity => entity.BookId)
                                            .ToHashSet();
        var exitingBooks = Books.Select(entity => entity.BookId)
                                .ToHashSet();

        var deletingBooks = AuthorBooks.Where(entity => !newBooks.Contains(entity.BookId))
                                       .ToList();

        foreach (var bookEntity in deletingBooks)
        {
          AuthorBooks.Remove(bookEntity);
        }

        var addingBooks = newAuthorEntity.Books.Where(entity => !exitingBooks.Contains(entity.BookId))
                                               .ToList();

        foreach (var bookEntity in addingBooks)
        {
          AuthorBooks.Add(new BookEntity(bookEntity));
        }
      }
      else
      {
        base.Update(newEntity, property);
      }
    }
  }
}
