﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Book;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Test.Book;
  using EfRepositorySample.Data.Book;
  using EfRepositorySample.Test.Book;
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

    [TestMethod]
    public async Task AddAsync_BookPassed_BookSaved()
    {
      var controlBookEntity = new TestBookEntity(
        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 500);

      var actualBookEntity =
        await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

      Assert.IsNotNull(actualBookEntity);

      var dbBookEntity =
        await DbContext.Set<BookEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == actualBookEntity.BookId)
                       .FirstOrDefaultAsync();

      Assert.IsNotNull(dbBookEntity);
      Assert.AreEqual(controlBookEntity.Title, dbBookEntity.Title);
      Assert.AreEqual(controlBookEntity.Description, dbBookEntity.Description);
      Assert.AreEqual(controlBookEntity.Pages, dbBookEntity.Pages);
    }

    [TestMethod]
    public async Task UpdateAsync_BookPassed_BookUpdated()
    {
      var originalBookEntity = await CreateBookAsync();
      var updatingBookEntity = new TestBookEntity(
        originalBookEntity.BookId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 800);
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
