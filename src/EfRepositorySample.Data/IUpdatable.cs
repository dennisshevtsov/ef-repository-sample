// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  /// <summary>Provides a simple API to update an entity.</summary>
  /// <typeparam name="TEntity">A type of an entity from which this entity should be updated.</typeparam>
  public interface IUpdatable<TEntity>
  {
    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    public void Update(TEntity newEntity);

    /// <summary>Updates this entity.</summary>
    /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    public void Update(TEntity newEntity, IEnumerable<string> properties);
  }
}
