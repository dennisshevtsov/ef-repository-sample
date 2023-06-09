﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Author.Data.Test;

public sealed class TestAuthorIdentity : IAuthorIdentity
{
  public TestAuthorIdentity(Guid authorId)
  {
    AuthorId = authorId;
  }

  public Guid AuthorId { get; }
}
