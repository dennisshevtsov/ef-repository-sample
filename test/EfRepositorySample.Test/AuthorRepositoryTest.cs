// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test
{
  using Microsoft.Extensions.DependencyInjection;

  using EfRepositorySample.Author;

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
  }
}
