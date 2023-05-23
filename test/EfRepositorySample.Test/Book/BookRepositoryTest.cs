// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Book;
  using EfRepositorySample.Author;
  using EfRepositorySample.Test.Author;
  using EfRepositorySample.Data.Author;

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
      var controlBookEntity = await TestBookEntity.AddAsync(DbContext);

      var actualBookEntity =
        await _bookRepository.GetAsync(controlBookEntity, Enumerable.Empty<string>(), CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.BookId, actualBookEntity.BookId);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
    }

    [TestMethod]
    public async Task GetAsync_AuthorsPropertyPassed_BookWithAuthorsReturned()
    {
      var controlAuthorEntityCollection = await TestAuthorEntity.AddAsync(DbContext, 5);
      var controlBookEntity = await TestBookEntity.AddAsync(DbContext, controlAuthorEntityCollection);
      var actualBookEntity =
        await _bookRepository.GetAsync(
          controlBookEntity,
          new[] { nameof(IBookEntity.Authors) },
          CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.BookId, actualBookEntity.BookId);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
      TestAuthorEntity.AreEqual(controlAuthorEntityCollection, actualBookEntity.Authors);
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
        await TestAuthorEntity.AddAsync(DbContext, 5));

      var savedBookEntity =
        await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

      Assert.IsNotNull(savedBookEntity);

      var actualBookEntity = await TestBookEntity.GetAsync(DbContext, savedBookEntity);

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
      TestAuthorEntity.AreEqual(controlBookEntity.Authors, actualBookEntity.Authors);
    }

    [TestMethod]
    public async Task UpdateAsync_BookPassed_BookUpdated()
    {
      var originalBookEntity = await TestBookEntity.AddAsync(DbContext);
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

      await _bookRepository.UpdateAsync(originalBookEntity, updatingBookEntity, updatingProperties, CancellationToken.None);

      var actualBookEntity = await TestBookEntity.GetAsync(DbContext, updatingBookEntity);

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(updatingBookEntity.BookId, actualBookEntity.BookId);
      Assert.AreEqual(updatingBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(updatingBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(updatingBookEntity.Pages, actualBookEntity.Pages);
    }

    [TestMethod]
    public async Task DeleteAsync_BookPassed_BookDeleted()
    {
      var controlBookEntity = await TestBookEntity.AddAsync(DbContext);

      await _bookRepository.DeleteAsync(controlBookEntity, CancellationToken.None);

      var actualBookEntity =
        await TestBookEntity.GetAsync(DbContext, controlBookEntity);

      Assert.IsNull(actualBookEntity);
    }
  }
}
