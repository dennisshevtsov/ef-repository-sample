// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Author;
  using EfRepositorySample.Data.Author;
  using EfRepositorySample.Test.Author;

  [TestClass]
  public sealed class BookRepositoryTest : IntegrationTestBase
  {
#pragma warning disable CS8618
    private IBookRepository _bookRepository;
#pragma warning restore CS8618

    protected override void Initialize(IServiceProvider provider)
    {
      _bookRepository = provider.GetRequiredService<IBookRepository>();
    }

    [TestMethod]
    public async Task GetAsync_UnknownBookId_NullReturned()
    {
      var controlBookIdentity = new TestBookIdentity(Guid.NewGuid());

      var actualBookEntity =
        await _bookRepository.GetAsync(controlBookIdentity, Enumerable.Empty<string>(), CancellationToken.None);

      Assert.IsNull(actualBookEntity);
    }

    [TestMethod]
    public async Task GetAsync_ExistingBookId_BookReturned()
    {
      var controlBookEntity = await CreateBookAsync();

      var actualBookEntity =
        await _bookRepository.GetAsync(controlBookEntity, Enumerable.Empty<string>(), CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.BookId, actualBookEntity.BookId);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
    }

    [TestMethod]
    public async Task AddAsync_BookPassed_SavedBookReturned()
    {
      var controlBookEntity = new TestBookEntity(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        500,
        new List<IAuthorEntity>());

      var actualBookEntity =
        await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);
      Assert.IsTrue(actualBookEntity.BookId != default);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
    }

    [TestMethod]
    public async Task AddAsync_BookPassed_BookSaved()
    {
      var controlBookEntity = new TestBookEntity(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        500,
        await CreateAuthorsAsync(5));

      var savedBookEntity =
        await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

      Assert.IsNotNull(savedBookEntity);

      var actualBookEntity =
        await DbContext.Set<BookEntity>()
                       .AsNoTracking()
                       .Include(entity => entity.BookAuthors)
                       .Where(entity => entity.Id == savedBookEntity.BookId)
                       .FirstOrDefaultAsync();

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
      TestAuthorEntity.AreEqual(controlBookEntity.Authors, actualBookEntity.Authors);
    }

    [TestMethod]
    public async Task UpdateAsync_BookPassed_BookUpdated()
    {
      var originalBookEntity = await CreateBookAsync();
      var updatingBookEntity = new TestBookEntity(
        originalBookEntity.BookId,
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        800,
        new List<IAuthorEntity>());
      var updatingProperties = new[]
      {
        nameof(IBookEntity.Title),
        nameof(IBookEntity.Description),
        nameof(IBookEntity.Pages),
      };

      await _bookRepository.UpdateAsync(updatingBookEntity, updatingProperties, CancellationToken.None);

      var actualBookEntity =
        await DbContext.Set<BookEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == updatingBookEntity.BookId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(updatingBookEntity.BookId, actualBookEntity.BookId);
      Assert.AreEqual(updatingBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(updatingBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(updatingBookEntity.Pages, actualBookEntity.Pages);
    }

    [TestMethod]
    public async Task DeleteAsync_BookPassed_BookDeleted()
    {
      var controlBookEntity = await CreateBookAsync();

      await _bookRepository.DeleteAsync(controlBookEntity, CancellationToken.None);

      var actualBookEntity =
        await DbContext.Set<BookEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == controlBookEntity.BookId)
                       .SingleOrDefaultAsync();

      Assert.IsNull(actualBookEntity);
    }

    private async Task<IBookEntity> CreateBookAsync()
    {
      var testBookEntity = TestBookEntity.New(500);
      var dataBookEntity = new BookEntity(testBookEntity);

      var dataBookEntityEntry = DbContext.Add(dataBookEntity);
      await DbContext.SaveChangesAsync();
      dataBookEntityEntry.State = EntityState.Detached;

      return dataBookEntity;
    }

    private async Task<IEnumerable<IAuthorEntity>> CreateAuthorsAsync(int authors)
    {
      var authorEntityCollection = new List<AuthorEntity>();

      for (int i = 0; i < authors; i++)
      {
        var testAuthorEntity = TestAuthorEntity.New();
        var dataAuthorEntity = new AuthorEntity(testAuthorEntity);

        authorEntityCollection.Add(dataAuthorEntity);
      }

      DbContext.AddRange(authorEntityCollection);
      await DbContext.SaveChangesAsync();

      foreach (var dataAuthorEntity in authorEntityCollection)
      {
        DbContext.Entry(dataAuthorEntity).State = EntityState.Detached;
      }

      return authorEntityCollection.Select(entity => new TestAuthorEntity(entity))
                                   .ToList();
    }
  }
}
