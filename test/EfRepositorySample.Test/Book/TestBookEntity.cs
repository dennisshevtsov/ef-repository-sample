// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Test.Author;
  using Microsoft.EntityFrameworkCore;

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

    public static TestBookEntity New(int pages, IEnumerable<IAuthorEntity> authors) =>
      new TestBookEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), pages, authors);

    public static TestBookEntity New(int pages) => TestBookEntity.New(pages, new List<IAuthorEntity>());

    public static async Task<IBookEntity> AddAsync(DbContext dbContext)
    {
      var testBookEntity = TestBookEntity.New(500);
      var dataBookEntity = new BookEntity(testBookEntity);

      var dataBookEntityEntry = dbContext.Add(dataBookEntity);
      await dbContext.SaveChangesAsync();
      dataBookEntityEntry.State = EntityState.Detached;

      return dataBookEntity;
    }

    public static async Task<IEnumerable<IBookEntity>> AddAsync(
      DbContext dbContext, int books)
    {
      var bookEntityCollection = new List<BookEntity>();

      for (int i = 0; i < books; i++)
      {
        var testBookEntity = TestBookEntity.New(i * 100);
        var dataBookEntity = new BookEntity(testBookEntity);

        bookEntityCollection.Add(dataBookEntity);
      }

      dbContext.AddRange(bookEntityCollection);
      await dbContext.SaveChangesAsync();

      foreach (var dataBookEntity in bookEntityCollection)
      {
        dbContext.Entry(dataBookEntity).State = EntityState.Detached;
      }

      return bookEntityCollection.Select(entity => new TestBookEntity(entity))
                                 .ToList();
    }

    public static async Task<IBookEntity?> GetAsync(DbContext dbContext, IBookIdentity identity) =>
      await dbContext.Set<BookEntity>()
                     .AsNoTracking()
                     .Include(entity => entity.BookAuthors)
                     .Where(entity => entity.Id == identity.BookId)
                     .FirstOrDefaultAsync();

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
