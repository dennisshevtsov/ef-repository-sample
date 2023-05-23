// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  /// <summary>Provides a simple API to update an entity.</summary>
  /// <typeparam name="TEntity">A type of an entity from that this entity is updated.</typeparam>
  public interface IUpdatable<TEntity>
  {
    /// <summary>Updates this entity.</summary>
    /// <param name="entity">An object that represents an entity from that this entity is updated.</param>
    public void Update(TEntity entity);
  }
}
