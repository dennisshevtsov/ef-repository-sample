// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using Microsoft.EntityFrameworkCore;

  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Author;

  public sealed class TestAuthorEntity : IAuthorEntity
  {
    public TestAuthorEntity(IAuthorEntity authorEntity)
      : this(authorEntity.AuthorId, authorEntity.Name, authorEntity.Bio, authorEntity.Books)
    { }

    public TestAuthorEntity(string name, string bio, IEnumerable<IBookEntity> books)
    {
      Name  = name;
      Bio   = bio;
      Books = books;
    }

    public TestAuthorEntity(Guid authorId, string name, string bio, IEnumerable<IBookEntity> books)
      : this(name, bio, books)
    {
      AuthorId = authorId;
    }

    public Guid AuthorId { get; }

    public string Name { get; }

    public string Bio { get; }

    public IEnumerable<IBookEntity> Books { get; }

    public static TestAuthorEntity New(IEnumerable<IBookEntity> books) =>
      new TestAuthorEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), books);

    public static TestAuthorEntity New() => New(new List<IBookEntity>());

    public static async Task<IAuthorEntity> AddAsync(DbContext dbContext, IEnumerable<IBookEntity> books)
    {
      var testAuthorEntity = TestAuthorEntity.New(books);
      var dataAuthorEntity = new AuthorEntity(testAuthorEntity);

      var dataAuthorEntityEntry = dbContext.Add(dataAuthorEntity);

      foreach (var bookEntity in dataAuthorEntity.AuthorBooks)
      {
        dbContext.Entry(bookEntity).State = EntityState.Unchanged;
      }

      await dbContext.SaveChangesAsync();
      dataAuthorEntityEntry.State = EntityState.Detached;

      foreach (var bookEntity in dataAuthorEntity.AuthorBooks)
      {
        dbContext.Entry(bookEntity).State = EntityState.Detached;
      }

      return dataAuthorEntity;
    }

    public static Task<IAuthorEntity> AddAsync(DbContext dbContext) =>
      TestAuthorEntity.AddAsync(dbContext, new List<IBookEntity>());

    public static async Task<IEnumerable<IAuthorEntity>> AddAsync(DbContext dbContext, int authors)
    {
      var authorEntityCollection = new List<AuthorEntity>();

      for (int i = 0; i < authors; i++)
      {
        var testAuthorEntity = TestAuthorEntity.New();
        var dataAuthorEntity = new AuthorEntity(testAuthorEntity);

        authorEntityCollection.Add(dataAuthorEntity);
      }

      dbContext.AddRange(authorEntityCollection);
      await dbContext.SaveChangesAsync();

      foreach (var dataAuthorEntity in authorEntityCollection)
      {
        dbContext.Entry(dataAuthorEntity).State = EntityState.Detached;
      }

      return authorEntityCollection.Select(entity => new TestAuthorEntity(entity))
                                   .ToList();
    }

    public static async Task<IAuthorEntity?> GetAsync(DbContext dbContext, IAuthorIdentity identity) =>
      await dbContext.Set<AuthorEntity>()
                     .AsNoTracking()
                     .Include(entity => entity.AuthorBooks)
                     .Where(entity => entity.Id == identity.AuthorId)
                     .FirstOrDefaultAsync();

    public static void AreEqual(
      IEnumerable<IAuthorEntity> controlAuthorEntityCollection,
      IEnumerable<IAuthorEntity> actualAuthorEntityCollection)
    {
      var controlAuthorEntityList =
        controlAuthorEntityCollection.OrderBy(entity => entity.AuthorId)
                                   .ToList();

      var actualAuthorEntityList =
        actualAuthorEntityCollection.OrderBy(entity => entity.AuthorId)
                                  .ToList();

      Assert.AreEqual(controlAuthorEntityList.Count, actualAuthorEntityList.Count);

      for (int i = 0; i < controlAuthorEntityList.Count; i++)
      {
        TestAuthorEntity.AreEqual(controlAuthorEntityList[i], actualAuthorEntityList[i]);
      }
    }

    public static void AreEqual(
      IAuthorEntity controlAuthorEntity,
      IAuthorEntity actualAuthorEntity)
    {
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    }
  }
}
