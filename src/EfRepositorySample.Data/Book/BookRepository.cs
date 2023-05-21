// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Book
{
  using Microsoft.EntityFrameworkCore;

  using EfRepositorySample.Book;
  using EfRepositorySample.Author;
  using EfRepositorySample.Data.Author;

  /// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Book.IBookEntity"/>.</summary>
  public sealed class BookRepository : RepositoryBase<BookEntity, IBookEntity, IBookIdentity>, IBookRepository
  {
    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Book.BookRepository"/> class.</summary>
    /// <param name="dbContext"></param>
    public BookRepository(DbContext dbContext) : base(dbContext) { }

    /// <summary>Includes relations.</summary>
    /// <param name="query">An object that represents a query of entities.</param>
    /// <param name="relations">An object that represents a collection of relations to load.</param>
    /// <returns>An object that represents a query of entities.</returns>
    protected override IQueryable<BookEntity> IncludeRelations(
      IQueryable<BookEntity> query, IEnumerable<string> relations)
    {
      if (relations.Contains(nameof(IBookEntity.Authors)))
      {
        query = query.Include(entity => entity.BookAuthors);
      }

      return query;
    }
  }
}
