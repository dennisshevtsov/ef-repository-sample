// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Test.Author
{
  using EfRepositorySample.Author;
  using EfRepositorySample.Book;

  public sealed class TestAuthorEntity : IAuthorEntity
  {
    public TestAuthorEntity(IAuthorEntity authorEntity)
      : this(authorEntity.Name, authorEntity.Bio, authorEntity.Books)
    { }

    public TestAuthorEntity(string name, string bio, IEnumerable<IBookEntity> books)
    {
      Name  = name;
      Bio   = bio;
      Books = books;
    }

    public TestAuthorEntity(Guid authorId, string name, string bio, IEnumerable<IBookEntity> books)
      : this(name, bio, books)
    {
      AuthorId = authorId;
    }

    public Guid AuthorId { get; }

    public string Name { get; }

    public string Bio { get; }

    public IEnumerable<IBookEntity> Books { get; }
  }
}
