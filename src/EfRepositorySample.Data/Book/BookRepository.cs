// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;

using EfRepositorySample.Data;

namespace EfRepositorySample.Book.Data;

/// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Book.IBookEntity"/>.</summary>
public sealed class BookRepository : RepositoryBase<BookEntity, IBookEntity, IBookIdentity>, IBookRepository
{
  /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Book.Data.BookRepository"/> class.</summary>
  /// <param name="dbContext"></param>
  public BookRepository(DbContext dbContext) : base(dbContext) { }
}
