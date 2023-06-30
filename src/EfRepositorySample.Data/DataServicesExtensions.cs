// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using EfRepositorySample.Author;
using EfRepositorySample.Author.Data;
using EfRepositorySample.Book;
using EfRepositorySample.Book.Data;
using EfRepositorySample.Data;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Extends an API of the <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.</summary>
public static class DataServicesExtensions
{
  /// <summary>Registers infrastructure services.</summary>
  /// <param name="services">An object that specifies the contract for a collection of service descriptors.</param>
  /// <param name="configuration">An object that represents a set of key/value application configuration properties.</param>
  /// <returns>An object that specifies the contract for a collection of service descriptors.</returns>
  public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<DatabaseOptions>(configuration);
    services.AddDbContext<DbContext, AppDbContext>((provider, builder) =>
    {
      var options = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
      ArgumentException.ThrowIfNullOrEmpty(options.ConnectionString);

      builder.UseNpgsql(options.ConnectionString);
    });

    services.AddRepositories();

    return services;
  }

  /// <summary>Registers infrastructure services.</summary>
  /// <param name="services">An object that specifies the contract for a collection of service descriptors.</param>
  /// <returns>An object that specifies the contract for a collection of service descriptors.</returns>
  public static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IAuthorRepository, AuthorRepository>();
    services.AddScoped<IBookRepository, BookRepository>();

    return services;
  }
}
