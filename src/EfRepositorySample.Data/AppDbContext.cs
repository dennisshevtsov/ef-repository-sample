// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  using Microsoft.EntityFrameworkCore;

  /// <summary>Represents a session with the database and can be used to query and save instances of your entities.</summary>
  public sealed class AppDbContext : DbContext
  {
  }
}
