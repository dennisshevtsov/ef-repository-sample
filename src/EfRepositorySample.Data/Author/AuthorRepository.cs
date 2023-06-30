// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;

using EfRepositorySample.Data;

namespace EfRepositorySample.Author.Data;

/// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Author.IAuthorEntity"/>.</summary>
public sealed class AuthorRepository : RepositoryBase<AuthorEntity, IAuthorEntity, IAuthorIdentity>, IAuthorRepository
{
  /// <summary>Initializes a new instance of the <see cref="EfRepositorySample.Author.Data.AuthorRepository"/> class.</summary>
  /// <param name="dbContext">An object that represents a session with the database and can be used to query and save instances of your entities.</param>
  public AuthorRepository(DbContext dbContext) : base(dbContext) { }
}
