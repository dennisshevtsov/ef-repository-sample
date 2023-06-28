// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using EfRepositorySample.Author;

namespace EfRepositorySample.Test.Author;

public sealed class TestAuthorIdentity : IAuthorIdentity
{
  public TestAuthorIdentity(Guid authorId)
  {
    AuthorId = authorId;
  }

  public Guid AuthorId { get; }
}
