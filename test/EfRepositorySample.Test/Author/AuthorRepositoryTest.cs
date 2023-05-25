// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Author;
  using EfRepositorySample.Book;
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
        await _authorRepository.GetAsync(controlAuthorIdentity, Enumerable.Empty<string>(), CancellationToken.None);

      Assert.IsNull(actualAuthorEntity);
    }

    [TestMethod]
    public async Task GetAsync_ExistingAuthorId_AuthorReturned()
    {
      var controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext);

      var actualAuthorEntity =
        await _authorRepository.GetAsync(controlAuthorEntity, Enumerable.Empty<string>(), CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    }

    [TestMethod]
    public async Task GetAsync_BooksPropertyPassed_AuthorWithBooksReturned()
    {
      var controlBookEntityCollection = await TestBookEntity.AddAsync(DbContext, 5);
      var controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext, controlBookEntityCollection);

      var actualAuthorEntity =
        await _authorRepository.GetAsync(
          controlAuthorEntity,
          new[] { nameof(IAuthorEntity.Books) },
          CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
      TestBookEntity.AreEqual(controlBookEntityCollection, actualAuthorEntity.Books);
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
        await TestBookEntity.AddAsync(DbContext, 5));

      var savedAuthorEntity =
        await _authorRepository.AddAsync(controlAuthorEntity, CancellationToken.None);

      Assert.IsNotNull(savedAuthorEntity);

      var actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, savedAuthorEntity);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
      TestBookEntity.AreEqual(controlAuthorEntity.Books, actualAuthorEntity.Books);
    }

    [TestMethod]
    public async Task UpdateAsync_AuthorPassed_AuthorUpdated()
    {
      var originalBookEntityCollection = await TestBookEntity.AddAsync(DbContext, 5);
      var originalAuthorEntity = await TestAuthorEntity.AddAsync(DbContext, originalBookEntityCollection);

      var updatingBookEntityCollection = new List<IBookEntity>
      {
        originalBookEntityCollection.First(),
        await TestBookEntity.AddAsync(DbContext),
        await TestBookEntity.AddAsync(DbContext),
      };
      var updatingAuthorEntity = new TestAuthorEntity(
        originalAuthorEntity.AuthorId,
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        updatingBookEntityCollection);
      var updatingProperties = new[]
      {
        nameof(IAuthorEntity.Name),
        nameof(IAuthorEntity.Bio),
        nameof(IAuthorEntity.Books),
      };

      await _authorRepository.UpdateAsync(originalAuthorEntity, updatingAuthorEntity, updatingProperties, CancellationToken.None);

      var actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, updatingAuthorEntity);

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(updatingAuthorEntity.AuthorId, actualAuthorEntity.AuthorId);
      Assert.AreEqual(updatingAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(updatingAuthorEntity.Bio, actualAuthorEntity.Bio);
      TestBookEntity.AreEqual(updatingBookEntityCollection, actualAuthorEntity.Books);
    }

    [TestMethod]
    public async Task DeleteAsync_AuthorPassed_AuthorDeleted()
    {
      var controlAuthorEntity = await TestAuthorEntity.AddAsync(DbContext);

      await _authorRepository.DeleteAsync(controlAuthorEntity, CancellationToken.None);

      var actualAuthorEntity = await TestAuthorEntity.GetAsync(DbContext, controlAuthorEntity);

      Assert.IsNull(actualAuthorEntity);
    }
  }
}
