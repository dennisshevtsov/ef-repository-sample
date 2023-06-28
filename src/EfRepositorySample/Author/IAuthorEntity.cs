// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using EfRepositorySample.Book;

namespace EfRepositorySample.Author;

/// <summary>Represents an author entity.</summary>
public interface IAuthorEntity : IAuthorIdentity
{
  /// <summary>Gets an object that represents a name of an author.</summary>
  public string Name { get; }

  /// <summary>Gets an object that represents a bio of an author.</summary>
  public string Bio { get; }

  /// <summary>Gets an object that represents a collection of this author's books.</summary>
  public IEnumerable<IBookEntity> Books { get; }
}
