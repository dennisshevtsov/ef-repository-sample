// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Book
{
  /// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Book.IBookEntity"/>.</summary>
  public interface IBookRepository : IRepository<IBookEntity, IBookIdentity>
  {
  }
}
