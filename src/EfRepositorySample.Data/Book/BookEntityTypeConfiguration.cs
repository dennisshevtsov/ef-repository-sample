// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Book
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  /// <summary>Defines an entity type configuration for the <see cref="EfRepositorySample.Data.Book.BookEntity"/>.</summary>
  public sealed class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
  {
    /// <summary>Configures the entity of type <see cref="EfRepositorySample.Data.Book.BookEntity"/>.</summary>
    /// <param name="builder">An object that provides a simple API for configuring an <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType" />.</param>
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
      builder.ToTable("author");
      builder.HasKey(entity => entity.Id);

      builder.Property(entity => entity.Id)
             .HasColumnName("id")
             .IsRequired();

      builder.Property(entity => entity.Title)
             .HasColumnName("title")
             .IsRequired()
             .HasMaxLength(255);

      builder.Property(entity => entity.Description)
             .HasColumnName("description")
             .IsRequired()
             .HasMaxLength(255);

      builder.Property(entity => entity.Pages)
             .HasColumnName("pages")
             .IsRequired();

      builder.Ignore(entity => entity.BookId);
    }
  }
}
