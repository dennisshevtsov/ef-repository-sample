﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfRepositorySample.Author.Data;

/// <summary>Defines an entity type configuration for the <see cref="EfRepositorySample.Author.Data.AuthorEntity"/>.</summary>
public sealed class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
  /// <summary>Configures the entity of type <see cref="EfRepositorySample.Author.Data.AuthorEntity"/>.</summary>
  /// <param name="builder">An object that provides a simple API for configuring an <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType" />.</param>
  public void Configure(EntityTypeBuilder<AuthorEntity> builder)
  {
    builder.ToTable("author");
    builder.HasKey(entity => entity.Id);

    builder.Property(entity => entity.Id)
           .HasColumnName("id")
           .IsRequired();

    builder.Property(entity => entity.Name)
           .HasColumnName("name")
           .IsRequired()
           .HasMaxLength(255);

    builder.Property(entity => entity.Bio)
           .HasColumnName("bio")
           .IsRequired()
           .HasMaxLength(255);

    builder.HasMany(entity => entity.AuthorBooks)
           .WithMany(entity => entity.BookAuthors)
           .UsingEntity<AuthorBookRelationEntity>(
             "author_book",
             builder => builder.HasOne(entity => entity.Book)
                               .WithMany()
                               .HasForeignKey(entity => entity.BookId)
                               .HasPrincipalKey(entity => entity.BookId),
             builder => builder.HasOne(entity => entity.Author)
                               .WithMany()
                               .HasForeignKey(entity => entity.AuthorId)
                               .HasPrincipalKey(entity => entity.AuthorId),
             builder =>
             {
               builder.Property(entity => entity.AuthorId)
                      .HasColumnName("author_id")
                      .IsRequired();

               builder.Property(entity => entity.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();
             });

    builder.Ignore(entity => entity.AuthorId);
    builder.Ignore(entity => entity.Books);
  }
}
