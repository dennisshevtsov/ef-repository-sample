// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using EfRepositorySample.Author;

  public sealed class TestAuthorEntity : IAuthorEntity
  {
    public TestAuthorEntity(string name, string bio)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Bio = bio ?? throw new ArgumentNullException(nameof(bio));
    }

    public TestAuthorEntity(Guid authorId, string name, string bio) : this(name, bio)
    {
      AuthorId = authorId;
    }

    public Guid AuthorId { get; }

    public string Name { get; }

    public string Bio { get; }
  }
}
