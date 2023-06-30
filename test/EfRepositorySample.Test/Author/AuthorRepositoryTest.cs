// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

using EfRepositorySample.Book;
using EfRepositorySample.Book.Data.Test;
using EfRepositorySample.Test;

namespace EfRepositorySample.Author.Data.Test;

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
    TestAuthorIdentity controlAuthorIdentity = new(Guid.NewGuid());

    IAuthorEntity? actualAuthorEntity = await _authorRepository.GetAsync(
      controlAuthorIdentity, Enumerable.Empty<string>(), CancellationToken.None);

    Assert.IsNull(actualAuthorEntity);
  }

  [TestMethod]
  public async Task GetAsync_ExistingAuthorId_AuthorReturned()
  {
    IAuthorEntity controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext);

    IAuthorEntity? actualAuthorEntity = await _authorRepository.GetAsync(
      controlAuthorEntity, Enumerable.Empty<string>(), CancellationToken.None);

    Assert.IsNotNull(actualAuthorEntity);
    Assert.AreEqual(controlAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
    Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
  }

  [TestMethod]
  public async Task GetAsync_BooksPropertyPassed_AuthorWithBooksReturned()
  {
    List<IBookEntity> controlBookEntityCollection = await TestBookEntity.AddAsync(DbContext, 5);
    IAuthorEntity controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext, controlBookEntityCollection);

    IAuthorEntity? actualAuthorEntity = await _authorRepository.GetAsync(
      controlAuthorEntity, new[] { nameof(IAuthorEntity.Books) }, CancellationToken.None);

    Assert.IsNotNull(actualAuthorEntity);
    Assert.AreEqual(controlAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
    Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    TestBookEntity.AreEqual(controlBookEntityCollection, actualAuthorEntity.Books);
  }

  [TestMethod]
  public async Task AddAsync_AuthorPassed_SavedAuthorReturned()
  {
    TestAuthorEntity controlAuthorEntity = new(
      Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new List<IBookEntity>());

    IAuthorEntity actualAuthorEntity = await _authorRepository.AddAsync(
      controlAuthorEntity, CancellationToken.None);

    Assert.IsNotNull(actualAuthorEntity);
    Assert.IsTrue(actualAuthorEntity.AuthorId != default);
    Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
  }

  [TestMethod]
  public async Task AddAsync_AuthorPassed_AuthorSaved()
  {
    TestAuthorEntity controlAuthorEntity = new(
      Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), await TestBookEntity.AddAsync(DbContext, 5));

    IAuthorEntity savedAuthorEntity = await _authorRepository.AddAsync(
      controlAuthorEntity, CancellationToken.None);

    Assert.IsNotNull(savedAuthorEntity);

    IAuthorEntity? actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, savedAuthorEntity);

    Assert.IsNotNull(actualAuthorEntity);
    Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    TestBookEntity.AreEqual(controlAuthorEntity.Books, actualAuthorEntity.Books);
  }

  [TestMethod]
  public async Task UpdateAsync_AuthorPassed_AuthorUpdated()
  {
    List<IBookEntity> controlBookEntityCollection = await TestBookEntity.AddAsync(DbContext, 5);

    List<IBookEntity> originalBookEntityCollection = new()
    {
      controlBookEntityCollection[0],
      controlBookEntityCollection[1],
      controlBookEntityCollection[2],
    };
    IAuthorEntity originalAuthorEntity = await TestAuthorEntity.AddAsync(DbContext, originalBookEntityCollection);

    List<IBookEntity> updatingBookEntityCollection = new()
    {
      controlBookEntityCollection[1],
      controlBookEntityCollection[3],
      controlBookEntityCollection[4],
    };
    TestAuthorEntity updatingAuthorEntity = new(
      originalAuthorEntity.AuthorId,
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString(),
      updatingBookEntityCollection);
    string[] updatingProperties = new[]
    {
      nameof(IAuthorEntity.Name),
      nameof(IAuthorEntity.Bio),
      nameof(IAuthorEntity.Books),
    };

    await _authorRepository.UpdateAsync(originalAuthorEntity, updatingAuthorEntity, updatingProperties, CancellationToken.None);

    IAuthorEntity? actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, updatingAuthorEntity);

    Assert.IsNotNull(actualAuthorEntity);
    Assert.AreEqual(updatingAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
    Assert.AreEqual(updatingAuthorEntity.Name, actualAuthorEntity.Name);
    Assert.AreEqual(updatingAuthorEntity.Bio, actualAuthorEntity.Bio);
    TestBookEntity.AreEqual(updatingBookEntityCollection, actualAuthorEntity.Books);
  }

  [TestMethod]
  public async Task DeleteAsync_AuthorPassed_AuthorDeleted()
  {
    IAuthorEntity controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext);

    await _authorRepository.DeleteAsync(controlAuthorEntity, CancellationToken.None);

    IAuthorEntity? actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, controlAuthorEntity);
    
    Assert.IsNull(actualAuthorEntity);
  }
}
