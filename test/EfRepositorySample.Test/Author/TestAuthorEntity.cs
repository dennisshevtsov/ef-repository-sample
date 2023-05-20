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

    public static TestAuthorEntity New() => new TestAuthorEntity(
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString(),
      new List<IBookEntity>());

    public static void AreEqual(
      IEnumerable<IAuthorEntity> controlAuthorEntityCollection,
      IEnumerable<IAuthorEntity> actualAuthorEntityCollection)
    {
      var controlAuthorEntityList =
        controlAuthorEntityCollection.OrderBy(entity => entity.AuthorId)
                                   .ToList();

      var actualAuthorEntityList =
        actualAuthorEntityCollection.OrderBy(entity => entity.AuthorId)
                                  .ToList();

      Assert.AreEqual(controlAuthorEntityList.Count, actualAuthorEntityList.Count);

      for (int i = 0; i < controlAuthorEntityList.Count; i++)
      {
        TestAuthorEntity.AreEqual(controlAuthorEntityList[i], actualAuthorEntityList[i]);
      }
    }

    public static void AreEqual(
      IAuthorEntity controlAuthorEntity,
      IAuthorEntity actualAuthorEntity)
    {
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
      Assert.AreEqual(controlAuthorEntity.Bio, actualAuthorEntity.Bio);
    }
  }
}
