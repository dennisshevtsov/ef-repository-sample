// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Author;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Test.Book;

  [TestClass]
  public sealed class AuthorRepositoryTest : IntegrationTestBase
  {
#pragma warning disable CS8618
    private IAuthorRepository _authorRepository;
#pragma warning restore CS8618

    protected override void Initialize(IServiceProvider provider)
    {
      _authorRepository = provider.GetRequiredService<IAuthorRepository>();
    }

    [TestMethod]
    public async Task GetAsync_UnknownAuthorId_NullReturned()
    {
      var controlAuthorIdentity = new TestAuthorIdentity(Guid.NewGuid());

      var actualAuthorEntity =
        await _authorRepository.GetAsync(controlAuthorIdentity, CancellationToken.None);

      Assert.IsNull(actualAuthorEntity);
    }

    [TestMethod]
    public async Task GetAsync_ExistingAuthorId_AuthorReturned()
    {
      var controlAuthorEntity = await CreateAuthorAsync();

      var actualAuthorEntity =
        await _authorRepository.GetAsync(controlAuthorEntity, CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    }

    [TestMethod]
    public async Task AddAsync_AuthorPassed_SavedAuthorReturned()
    {
      var controlAuthorEntity = new TestAuthorEntity(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        new List<IBookEntity>());

      var actualAuthorEntity =
        await _authorRepository.AddAsync(controlAuthorEntity, CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.IsTrue(actualAuthorEntity.AuthorId != default);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    }

    [TestMethod]
    public async Task AddAsync_AuthorPassed_AuthorSaved()
    {
      var controlAuthorEntity = new TestAuthorEntity(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        await CreateBooksAsync(5));

      var actualAuthorEntity =
        await _authorRepository.AddAsync(controlAuthorEntity, CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);

      var dbAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Include(entity => entity.AuthorBooks)
                       .Where(entity => entity.Id == actualAuthorEntity.AuthorId)
                       .FirstOrDefaultAsync();

      Assert.IsNotNull(dbAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.Name, dbAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio , dbAuthorEntity.Bio );
      TestBookEntity.AreEqual(controlAuthorEntity.Books, actualAuthorEntity.Books);
    }

    [TestMethod]
    public async Task UpdateAsync_AuthorPassed_AuthorUpdated()
    {
      var originalAuthorEntity = await CreateAuthorAsync();
      var updatingAuthorEntity = new TestAuthorEntity(
        originalAuthorEntity.AuthorId,
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        new List<IBookEntity>());
      var updatingProperties = new[]
      {
        nameof(IAuthorEntity.Name),
        nameof(IAuthorEntity.Bio),
      };

      await _authorRepository.UpdateAsync(updatingAuthorEntity, updatingProperties, CancellationToken.None);

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == updatingAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(updatingAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
      Assert.AreEqual(updatingAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(updatingAuthorEntity.Bio, actualAuthorEntity.Bio);
    }

    [TestMethod]
    public async Task DeleteAsync_AuthorPassed_AuthorDeleted()
    {
      var controlAuthorEntity = await CreateAuthorAsync();

      await _authorRepository.DeleteAsync(controlAuthorEntity, CancellationToken.None);

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNull(actualAuthorEntity);
    }

    private async Task<IAuthorEntity> CreateAuthorAsync()
    {
      var testAuthorEntity = new TestAuthorEntity(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        new List<IBookEntity>());
      var dataAuthorEntity = new AuthorEntity(testAuthorEntity);

      var dataAuthorEntityEntry = DbContext.Add(dataAuthorEntity);
      await DbContext.SaveChangesAsync();
      dataAuthorEntityEntry.State = EntityState.Detached;

      return dataAuthorEntity;
    }

    private async Task<IEnumerable<IBookEntity>> CreateBooksAsync(int books)
    {
      var bookEntityCollection = new List<BookEntity>();

      for (int i = 0; i < books; i++)
      {
        var testBookEntity = TestBookEntity.New(i * 100);
        var dataBookEntity = new BookEntity(testBookEntity);

        bookEntityCollection.Add(dataBookEntity);
      }

      DbContext.AddRange(bookEntityCollection);
      await DbContext.SaveChangesAsync();

      return bookEntityCollection.Select(entity => new TestBookEntity(entity))
                                 .ToList();
    }
  }
}
