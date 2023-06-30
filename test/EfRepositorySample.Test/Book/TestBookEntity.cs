// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using EfRepositorySample.Author;
using EfRepositorySample.Author.Data;
using EfRepositorySample.Author.Data.Test;

namespace EfRepositorySample.Book.Data.Test;

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

  public static async Task<IBookEntity> AddAsync(DbContext dbContext, IEnumerable<IAuthorEntity> authors)
  {
    TestBookEntity testBookEntity = TestBookEntity.New(500, authors);
    BookEntity dataBookEntity = new(testBookEntity);

    EntityEntry<BookEntity> dataBookEntityEntry = dbContext.Add(dataBookEntity);

    foreach (AuthorEntity authorEntity in dataBookEntity.BookAuthors)
    {
      dbContext.Entry(authorEntity).State = EntityState.Unchanged;
    }

    await dbContext.SaveChangesAsync();
    dataBookEntityEntry.State = EntityState.Detached;

    foreach (AuthorEntity authorEntity in dataBookEntity.BookAuthors)
    {
      dbContext.Entry(authorEntity).State = EntityState.Detached;
    }

    return dataBookEntity;
  }

  public static Task<IBookEntity> AddAsync(DbContext dbContext) =>
    AddAsync(dbContext, new List<IAuthorEntity>());

  public static async Task<List<IBookEntity>> AddAsync(
    DbContext dbContext, int books)
  {
    List<BookEntity> bookEntityCollection = new();

    for (int i = 0; i < books; i++)
    {
      TestBookEntity testBookEntity = TestBookEntity.New(i * 100);
      BookEntity dataBookEntity     = new(testBookEntity);

      bookEntityCollection.Add(dataBookEntity);
    }

    dbContext.AddRange(bookEntityCollection);
    await dbContext.SaveChangesAsync();

    foreach (var dataBookEntity in bookEntityCollection)
    {
      dbContext.Entry(dataBookEntity).State = EntityState.Detached;
    }

    return bookEntityCollection.Select(entity => new TestBookEntity(entity))
                               .OfType<IBookEntity>()
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
    List<IBookEntity> controlBookEntityList =
      controlBookEntityCollection.OrderBy(entity => entity.BookId)
                                 .ToList();

    List<IBookEntity> actualBookEntityList =
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
