// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Book
{
  /// <summary>Represents a book entity.</summary>
  public interface IBookEntity : IBookIdentity
  {
    /// <summary>Gets an object that represents a title of a book.</summary>
    public string Title { get; }

    /// <summary>Gets an object that represents a description of a book.</summary>
    public string Description { get; }

    /// <summary>Gets an object that represents a number of pages of a book.</summary>
    public int Pages { get; set; }
  }
}
