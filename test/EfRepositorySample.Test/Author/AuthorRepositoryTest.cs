// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Author;
  using EfRepositorySample.Data.Author;

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
        Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

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
        Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

      var actualAuthorEntity =
        await _authorRepository.AddAsync(controlAuthorEntity, CancellationToken.None);

      Assert.IsNotNull(actualAuthorEntity);

      var dbAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.Id == actualAuthorEntity.AuthorId)
                       .FirstOrDefaultAsync();

      Assert.IsNotNull(dbAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.Name, dbAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, dbAuthorEntity.Bio);
    }

    [TestMethod]
    public async Task UpdateAsync_AuthorPassed_AuthorUpdated()
    {
      var originalAuthorEntity = await CreateAuthorAsync();
      var updatingAuthorEntity = new TestAuthorEntity(
        originalAuthorEntity.AuthorId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
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
      var testAuthorEntity = new TestAuthorEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
      var dataAuthorEntity = new AuthorEntity(testAuthorEntity);

      var dataAuthorEntityEntry = DbContext.Add(dataAuthorEntity);
      await DbContext.SaveChangesAsync();
      dataAuthorEntityEntry.State = EntityState.Detached;

      return dataAuthorEntity;
    }
  }
}
