// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using EfRepositorySample.Book;

namespace EfRepositorySample.Test.Book;

public sealed class TestBookIdentity : IBookIdentity
{
  public TestBookIdentity(Guid bookId)
  {
    BookId = bookId;
  }

  public Guid BookId { get; }
}
