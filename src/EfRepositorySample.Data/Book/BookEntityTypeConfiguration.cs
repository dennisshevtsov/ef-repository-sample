﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfRepositorySample.Book.Data;

/// <summary>Defines an entity type configuration for the <see cref="EfRepositorySample.Book.Data.BookEntity"/>.</summary>
public sealed class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
{
  /// <summary>Configures the entity of type <see cref="EfRepositorySample.Book.Data.BookEntity"/>.</summary>
  /// <param name="builder">An object that provides a simple API for configuring an <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType" />.</param>
  public void Configure(EntityTypeBuilder<BookEntity> builder)
  {
    builder.ToTable("boock");
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

    builder.HasMany(entity => entity.BookAuthors)
           .WithMany(entity => entity.AuthorBooks);

    builder.Ignore(entity => entity.BookId);
    builder.Ignore(entity => entity.Authors);
  }
}
