// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Book
{
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Book;
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
        await _bookRepository.GetAsync(controlBookIdentity, CancellationToken.None);

      Assert.IsNull(actualBookEntity);
    }
  }
}
