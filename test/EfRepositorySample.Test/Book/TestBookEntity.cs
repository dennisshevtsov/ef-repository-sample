// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Test.Author;

  public sealed class TestBookEntity : IBookEntity
  {
    public TestBookEntity(IBookEntity bookEntity)
      : this(bookEntity.BookId,
             bookEntity.Title,
             bookEntity.Description,
             bookEntity.Pages,
             bookEntity.Authors)
    { }

    public TestBookEntity(string title, string description, int pages, IEnumerable<IAuthorEntity> authors)
    {
      Title       = title;
      Description = description;
      Pages       = pages;
      Authors     = authors.Select(entity => new TestAuthorEntity(entity))
                           .ToList();
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

    public static TestBookEntity New(int pages) => new TestBookEntity(
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString(),
      pages,
      new List<IAuthorEntity>());

    public static void AreEqual(
      IEnumerable<IBookEntity> controlBookEntityCollection,
      IEnumerable<IBookEntity> actualBookEntityCollection)
    {
      var controlBookEntityList =
        controlBookEntityCollection.OrderBy(entity => entity.BookId)
                                   .ToList();

      var actualBookEntityList =
        actualBookEntityCollection.OrderBy(entity => entity.BookId)
                                  .ToList();

      Assert.AreEqual(controlBookEntityList.Count, actualBookEntityList.Count);

      for (int i = 0; i < controlBookEntityList.Count; i++)
      {
        TestBookEntity.AreEqual(controlBookEntityList[i], actualBookEntityList[i]);
      }
    }

    public static void AreEqual(IBookEntity control, IBookEntity actual)
    {
      Assert.AreEqual(control.Title, actual.Title);
      Assert.AreEqual(control.Description, actual.Description);
      Assert.AreEqual(control.Pages, actual.Pages);
    }
  }
}
