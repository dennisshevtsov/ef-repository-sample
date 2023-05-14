// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  /// <summary>Represents database options.</summary>
  public sealed class DbOptions
  {
    /// <summary>Gets/sets an object that represents a connection string.</summary>
    public string? ConnectionString { get; set; }
  }
}
