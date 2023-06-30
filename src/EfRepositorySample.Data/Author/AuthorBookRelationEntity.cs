// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using EfRepositorySample.Book;
using EfRepositorySample.Book.Data;

namespace EfRepositorySample.Author.Data;

/// <summary>Represents an author book relation entity.</summary>
public sealed class AuthorBookRelationEntity : IAuthorIdentity, IBookIdentity
{
  /// <summary>Gets an object that represents an ID of an author.</summary>
  public Guid AuthorId { get; }

  /// <summary>Gets an object that represents an author.</summary>
  public AuthorEntity? Author { get; }

  /// <summary>Gets an object that represents an ID of a book.</summary>
  public Guid BookId { get; }

  /// <summary>Gets an object that represents a book.</summary>
  public BookEntity? Book { get; }
}
