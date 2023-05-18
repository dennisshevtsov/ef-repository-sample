// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;

  public sealed class TestBookEntity : IBookEntity
  {
    public TestBookEntity(string title, string description, int pages, IEnumerable<IAuthorEntity> authors)
    {
      Title       = title;
      Description = description;
      Pages       = pages;
      Authors     = authors;
    }

    public TestBookEntity(Guid bookId, string title, string description, int pages, IEnumerable<IAuthorEntity> authors)
      : this(title, description, pages, authors)
    {
      BookId = bookId;
    }

    public Guid BookId { get; }

    public string Title { get; }

    public string Description { get; }

    public int Pages { get; }

    public IEnumerable<IAuthorEntity> Authors { get; }
  }
}
