// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test
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
    public async Task AddAsync_AuthorEntityPassed_SavedAuthorEntityReturned()
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
  }
}
