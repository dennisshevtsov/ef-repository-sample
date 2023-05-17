// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Test.Book;

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
        await _bookRepository.GetAsync(controlBookIdentity, CancellationToken.None);

      Assert.IsNull(actualBookEntity);
    }

    [TestMethod]
    public async Task GetAsync_ExistingBookId_BookReturned()
    {
      var controlBookEntity = await CreateBookAsync();

      var actualBookEntity =
        await _bookRepository.GetAsync(controlBookEntity, CancellationToken.None);

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
        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 500);

      var actualBookEntity =
        await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);
      Assert.IsTrue(actualBookEntity.BookId != default);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
    }

    private async Task<IBookEntity> CreateBookAsync()
    {
      var testBookEntity = new TestBookEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 500);
      var dataBookEntity = new BookEntity(testBookEntity);

      var dataBookEntityEntry = DbContext.Add(dataBookEntity);
      await DbContext.SaveChangesAsync();
      dataBookEntityEntry.State = EntityState.Detached;

      return dataBookEntity;
    }
  }
}
