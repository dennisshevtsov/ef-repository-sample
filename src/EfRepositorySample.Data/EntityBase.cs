// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  /// <summary>Represents an entity base.</summary>
  public abstract class EntityBase
  {
    /// <summary>Gets an object that represents an ID of an entity.</summary>
    public Guid Id { get; protected set; }
  }
}
