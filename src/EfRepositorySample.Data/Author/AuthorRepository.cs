// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Author
{
  using Microsoft.EntityFrameworkCore;

  using EfRepositorySample.Author;

  /// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Author.IAuthorEntity"/>.</summary>
  public sealed class AuthorRepository : RepositoryBase<AuthorEntity, IAuthorEntity, IAuthorIdentity>, IAuthorRepository
  {
    /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Data.Author.AuthorRepository"/> class.</summary>
    /// <param name="dbContext">An object that represents a session with the database and can be used to query and save instances of your entities.</param>
    public AuthorRepository(DbContext dbContext) : base(dbContext) { }
  }
}
