// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Author
{
  /// <summary>Represents an author identity.</summary>
  public interface IAuthorIdentity
  {
    /// <summary>Gets an object that represents an ID of an author.</summary>
    public Guid AuthorId { get; }
  }
}
