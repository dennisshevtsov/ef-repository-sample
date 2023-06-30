// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

using EfRepositorySample.Author;
using EfRepositorySample.Author.Data.Test;
using EfRepositorySample.Test;

namespace EfRepositorySample.Book.Data.Test;

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
    TestBookIdentity controlBookIdentity = new(Guid.NewGuid());

    IBookEntity? actualBookEntity =
      await _bookRepository.GetAsync(controlBookIdentity, Enumerable.Empty<string>(), CancellationToken.None);

    Assert.IsNull(actualBookEntity);
  }

  [TestMethod]
  public async Task GetAsync_ExistingBookId_BookReturned()
  {
    IBookEntity controlBookEntity = await TestBookEntity.AddAsync(DbContext);

    IBookEntity? actualBookEntity = await _bookRepository.GetAsync(
      controlBookEntity, Enumerable.Empty<string>(), CancellationToken.None);

    Assert.IsNotNull(actualBookEntity);
    Assert.AreEqual(controlBookEntity.BookId, actualBookEntity.BookId);
    Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
    Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
    Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
  }

  [TestMethod]
  public async Task GetAsync_AuthorsPropertyPassed_BookWithAuthorsReturned()
  {
    List<IAuthorEntity> controlAuthorEntityCollection = await TestAuthorEntity.AddAsync(DbContext, 5);
    IBookEntity controlBookEntity = await TestBookEntity.AddAsync(DbContext, controlAuthorEntityCollection);

    IBookEntity? actualBookEntity = await _bookRepository.GetAsync(controlBookEntity, new[] { nameof(IBookEntity.Authors) }, CancellationToken.None);

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
    IBookEntity controlBookEntity = TestBookEntity.New(500, new List<IAuthorEntity>());

    IBookEntity actualBookEntity =
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
    IBookEntity controlBookEntity = TestBookEntity.New(
      500, await TestAuthorEntity.AddAsync(DbContext, 5));

    IBookEntity savedBookEntity =
      await _bookRepository.AddAsync(controlBookEntity, CancellationToken.None);

    Assert.IsNotNull(savedBookEntity);

    IBookEntity? actualBookEntity = await TestBookEntity.GetAsync(DbContext, savedBookEntity);

    Assert.IsNotNull(actualBookEntity);
    Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
    Assert.AreEqual(controlBookEntity.Description, actualBookEntity.Description);
    Assert.AreEqual(controlBookEntity.Pages, actualBookEntity.Pages);
    TestAuthorEntity.AreEqual(controlBookEntity.Authors, actualBookEntity.Authors);
  }

  [TestMethod]
  public async Task UpdateAsync_BookPassed_BookUpdated()
  {
    List<IAuthorEntity> controlAuthorEntityCollection = await TestAuthorEntity.AddAsync(DbContext, 5);

    List<IAuthorEntity> originalAuthorEntityCollection = new()
    {
      controlAuthorEntityCollection[0],
      controlAuthorEntityCollection[1],
      controlAuthorEntityCollection[2],
    };
    IBookEntity originalBookEntity = await TestBookEntity.AddAsync(DbContext, originalAuthorEntityCollection);

    List<IAuthorEntity> updatingAuthorEntityCollection = new()
    {
      controlAuthorEntityCollection[1],
    };
    TestBookEntity updatingBookEntity = new(
      originalBookEntity.BookId,
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString(),
      800,
      updatingAuthorEntityCollection);
    string[] updatingProperties = new[]
    {
      nameof(IBookEntity.Title),
      nameof(IBookEntity.Description),
      nameof(IBookEntity.Pages),
      nameof(IBookEntity.Authors),
    };

    await _bookRepository.UpdateAsync(originalBookEntity, updatingBookEntity, updatingProperties, CancellationToken.None);

    IBookEntity? actualBookEntity = await TestBookEntity.GetAsync(DbContext, updatingBookEntity);

    Assert.IsNotNull(actualBookEntity);
    Assert.AreEqual(updatingBookEntity.BookId, actualBookEntity.BookId);
    Assert.AreEqual(updatingBookEntity.Title, actualBookEntity.Title);
    Assert.AreEqual(updatingBookEntity.Description, actualBookEntity.Description);
    Assert.AreEqual(updatingBookEntity.Pages, actualBookEntity.Pages);
    TestAuthorEntity.AreEqual(updatingAuthorEntityCollection, actualBookEntity.Authors);
  }

  [TestMethod]
  public async Task DeleteAsync_BookPassed_BookDeleted()
  {
    IBookEntity controlBookEntity = await TestBookEntity.AddAsync(DbContext);

    await _bookRepository.DeleteAsync(controlBookEntity, CancellationToken.None);

    IBookEntity? actualBookEntity =
      await TestBookEntity.GetAsync(DbContext, controlBookEntity);

    Assert.IsNull(actualBookEntity);
  }
}
