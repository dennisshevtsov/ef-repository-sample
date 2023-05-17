// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using EfRepositorySample.Book;

  public sealed class TestBookEntity : IBookEntity
  {
    public TestBookEntity(string title, string description, int pages)
    {
      Title       = title;
      Description = description;
      Pages       = pages;
    }

    public TestBookEntity(Guid bookId, string title, string description, int pages)
      : this(title, description, pages)
    {
      BookId = bookId;
    }

    public Guid BookId { get; }

    public string Title { get; }

    public string Description { get; }

    public int Pages { get; }
  }
}
