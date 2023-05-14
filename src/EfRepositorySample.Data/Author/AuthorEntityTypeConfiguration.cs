// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data.Author
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  /// <summary>Defines an entity type configuration for the <see cref="EfRepositorySample.Data.Author.AuthorEntity"/>.</summary>
  public sealed class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<AuthorEntity>
  {
    /// <summary>Configures the entity of type <see cref="EfRepositorySample.Data.Author.AuthorEntity"/>.</summary>
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

      builder.Ignore(entity => entity.AuthorId);
    }
  }
}
