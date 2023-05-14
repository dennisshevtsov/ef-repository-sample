// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Author;
  using EfRepositorySample.Data.Book;

  /// <summary>Represents an entity base.</summary>
  public abstract class EntityBase
  {
    /// <summary>Gets an object that represents an ID of an entity.</summary>
    public Guid Id { get; protected set; }

    /// <summary>Creates a copy of an entity.</summary>
    /// <param name="entity">An object that represents an entity to copy.</param>
    /// <returns>An object that represents an instance of an entity copy.</returns>
    /// <exception cref="System.NotSupportedException">Throws if there is no such entity.</exception>
    public static EntityBase Create(object entity)
    {
      ArgumentNullException.ThrowIfNull(entity);

      if (entity is IAuthorEntity authorEntity)
      {
        return new AuthorEntity(authorEntity);
      }

      if (entity is IBookEntity bookEntity)
      {
        return new BookEntity(bookEntity);
      }

      throw new NotSupportedException("Unsupported entity type.");
    }
  }
}
